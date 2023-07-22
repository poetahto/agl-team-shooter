using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using UnityEngine;

public class FishNetWrapper
{
    private NetworkManager _network;
    private ServerManager _serverManager;
    private ClientManager _clientManager;
    private SceneManager _sceneManager;

    public FishNetWrapper()
    {
        _network = InstanceFinder.NetworkManager;
        _serverManager = _network.ServerManager;
        _clientManager = _network.ClientManager;
        _sceneManager = _network.SceneManager;
    }

    public bool IsClient => _network.IsClient;
    public bool IsServer => _network.IsServer;
    public bool IsHost => _network.IsHost;

    public async UniTask StartHost(ushort port)
    {
        // Start the server first.
        await StartServer(port);

        // Then start the local client.
        _clientManager.StartConnection();
        await UniTask.WaitUntil(() => _clientManager.Started);

        // And ensure stuff has updated.
        await UniTask.WaitUntil(() => _network.IsHost);
    }

    public async UniTask StartClient(string address, ushort port)
    {
        bool doneLoading = false;

        void SignalDoneLoading(NetworkConnection connection, bool asServer)
        {
            doneLoading = connection == _clientManager.Connection && !asServer;
            _network.SceneManager.OnClientLoadedStartScenes -= SignalDoneLoading;
        }

        _network.SceneManager.OnClientLoadedStartScenes += SignalDoneLoading;

        // Start the client connection.
        _clientManager.StartConnection(address, port);
        await UniTask.WaitUntil(() => _clientManager.Started);

        // Make sure we finish loading the initial scenes.
        await UniTask.WaitUntil(() => doneLoading);

        // And ensure stuff has updated.
        await UniTask.WaitUntil(() => _network.IsClient);
    }

    public async UniTask StartServer(ushort port)
    {
        _serverManager.StartConnection(port);
        await UniTask.WaitUntil(() => _serverManager.Started);

        // And ensure stuff has updated.
        await UniTask.WaitUntil(() => _network.IsServer);
    }

    public async UniTask Stop()
    {
        if (_clientManager.Started)
        {
            _clientManager.StopConnection();
            await UniTask.WaitUntil(() => !_clientManager.Started);
        }

        if (_serverManager.Started)
        {
            _serverManager.StopConnection(true);
            await UniTask.WaitUntil(() => !_serverManager.Started);
        }
    }

    // todo: I think this might not work for clients...
    public async UniTask LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        IDisposable loadingScreen = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        _sceneManager.LoadGlobalScenes(new SceneLoadData(sceneName){ReplaceScenes = ReplaceOption.All});
        loadingScreen.Dispose();
    }
}
