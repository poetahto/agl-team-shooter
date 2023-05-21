using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public interface IGameplaySystem
{
    bool TryGetServices(out IGameplayServices services);
    UniTask HostGame(ushort post);
    UniTask ConnectToGame(string address, ushort port);
    UniTask StopGame();
}

public class GameplaySystem : MonoBehaviour, IGameplaySystem
{
    [SerializeField]
    private GameplayServices gameplayServicesPrefab;

    private bool _isRunning;
    private IGameplayServices _services;
    private GameplayServices _serviceInstance;
    private IClient _activeClient;

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
        _activeClient = await _services.NetworkInterface.Host(port);
        _services.Lobby.AddClient(_activeClient);
        print($"hosted game with client {_activeClient.Id}");
        await _services.NetworkInterface.LoadScene("TestingMap");
    }

    public async UniTask ConnectToGame(string address, ushort port)
    {
        InitializeServices();
        _activeClient = await _services.NetworkInterface.Connect(address, port);
        _services.Lobby.AddClient(_activeClient);
        print($"joined game with client {_activeClient.Id}");
    }

    public async UniTask StopGame()
    {
        Debug.Log("Stopping game.");
        _isRunning = false;
        _services.NetworkInterface.Disconnect();
        var loadingScreen = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        Destroy(_serviceInstance.gameObject);
        await SceneManager.LoadSceneAsync("MainMenu");
        loadingScreen.Dispose();
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
