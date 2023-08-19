using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Object;
using FishNet.Utility;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class GameplayRunner : MonoBehaviour
{
    [SerializeField]
    private NetworkObject gameplaySystemsPrefab;

    [Scene]
    [SerializeField]
    private string initialServerScene;

    [Scene]
    [SerializeField]
    private string menuScene;

    private bool _isRunning;
    private FishNetWrapper _network;
    public FishNetSceneLoader SceneLoader;

    private void Awake()
    {
        Services.GameplayRunner = this;
        _network = new FishNetWrapper();
        SceneLoader = new FishNetSceneLoader();
    }

    public async UniTask HostGame(ushort port)
    {
        using var _ = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        await _network.StartHost(port);
        InitializeServices();
        await SceneLoader.Server_LoadScene(initialServerScene);
    }

    public async UniTask ConnectToGame(string address, ushort port)
    {
        using var _ = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        await _network.StartClient(address, port);
    }

    public async UniTask StopGame()
    {
        _isRunning = false;
        using var _ = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        await _network.Stop();
        await SceneManager.LoadSceneAsync(menuScene);
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
