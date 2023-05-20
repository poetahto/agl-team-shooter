using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IGameplaySystem
{
    bool TryGetServices(out IGameplayServices services);
    UniTask HostGame(ushort post);
    UniTask ConnectToGame(string address, ushort port);
    void StopGame();
}

public class GameplaySystem : MonoBehaviour, IGameplaySystem
{
    [SerializeField]
    private GameplayServices gameplayServicesPrefab;

    private bool _isRunning;
    private IGameplayServices _services;
    private GameplayServices _serviceInstance;

    private void Awake()
    {
        Services.GameplaySystem = this;
    }

    public bool TryGetServices(out IGameplayServices services)
    {
        if (_isRunning)
        {
            services = _services;
            return true;
        }

        services = null;
        return false;
    }

    public async UniTask HostGame(ushort port)
    {
        InitializeServices();
        await _services.NetworkInterface.Host(port);
        _services.NetworkInterface.LoadScene("TestingMap");
    }

    public async UniTask ConnectToGame(string address, ushort port)
    {
        InitializeServices();
        await _services.NetworkInterface.Connect(address, port);
    }

    public void StopGame()
    {
        _isRunning = false;
        Destroy(_serviceInstance);
    }

    private void InitializeServices()
    {
        if (_isRunning)
            throw new Exception("Game is already running!");

        _serviceInstance = Instantiate(gameplayServicesPrefab, transform);
        _services = _serviceInstance.GetComponent<IGameplayServices>();
        _isRunning = true;
    }
}
