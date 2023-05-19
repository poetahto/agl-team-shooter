using System;
using UnityEngine;

public interface IGameplaySystem
{
    bool TryGetServices(out IGameplayServices services);
    void HostGame(ushort post);
    void ConnectToGame(string address, ushort port);
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

    public void HostGame(ushort port)
    {
        InitializeServices();
        _services.NetworkInterface.Host(port);
    }

    public void ConnectToGame(string address, ushort port)
    {
        InitializeServices();
        _services.NetworkInterface.Connect(address, port);
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
