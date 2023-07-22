using System;
using Cysharp.Threading.Tasks;
using FishNet;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class GameplayRunner : MonoBehaviour
{
    [SerializeField]
    private GameplaySystems gameplaySystemsPrefab;

    private bool _isRunning;
    private FishNetWrapper _network;

    public GameplaySystems Systems { get; set; }

    private void Awake()
    {
        Services.GameplayRunner = this;
        _network = new FishNetWrapper();
    }

    public async UniTask HostGame(ushort port)
    {
        await _network.StartHost(port);
        InitializeServices();
        await _network.LoadScene("TestingMap");
    }

    public async UniTask ConnectToGame(string address, ushort port)
    {
        await _network.StartClient(address, port);
    }

    public async UniTask StopGame()
    {
        _isRunning = false;
        await _network.Stop();
        var loadingScreen = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        await SceneManager.LoadSceneAsync("MainMenu");
        loadingScreen.Dispose();
    }

    private void InitializeServices()
    {
        if (_isRunning)
            throw new Exception("Game is already running!");

        if (!_network.IsServer)
            throw new Exception("Only the server can initialize services!");

        var instance = Instantiate(gameplaySystemsPrefab);
        InstanceFinder.ServerManager.Spawn(instance.gameObject);
        _isRunning = true;
    }
}
