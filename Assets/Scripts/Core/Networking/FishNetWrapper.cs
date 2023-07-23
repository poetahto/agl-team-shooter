using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Object;
using UnityEngine;

public class FishNetSceneLoader : IDisposable
{
    private readonly NetworkManager _network;
    private readonly SceneManager _sceneManager;
    private bool _isLoading;

    public FishNetSceneLoader()
    {
        _network = InstanceFinder.NetworkManager;
        _sceneManager = _network.SceneManager;

        _sceneManager.OnQueueEnd += HandleSceneQueueEnd;
        _network.ClientManager.RegisterBroadcast<ClientHideScreenCommand>(Client_HandleHideScreen);
    }

    public void Dispose()
    {
        _sceneManager.OnQueueEnd -= HandleSceneQueueEnd;
        _network.ClientManager.UnregisterBroadcast<ClientHideScreenCommand>(Client_HandleHideScreen);
    }

    [Server]
    public async UniTask Server_LoadScene(string sceneName)
    {
        _isLoading = true;
        _sceneManager.LoadGlobalScenes(new SceneLoadData(sceneName){ReplaceScenes = ReplaceOption.All});
        _network.ServerManager.Broadcast(new ClientHideScreenCommand());
        await UniTask.WaitUntil(() => !_isLoading);
    }

    [Client]
    private void Client_HandleHideScreen(ClientHideScreenCommand _)
    {
        Client_HideScreenTask().Forget();
    }

    [Client]
    private async UniTask Client_HideScreenTask()
    {
        IDisposable loadingScreen = await Services.LoadingScreenFactory.FadeColor(Color.black, 0.1f);
        await UniTask.WaitUntil(() => !_isLoading);
        loadingScreen.Dispose();
    }

    private void HandleSceneQueueEnd()
    {
        _isLoading = false;
    }

    private struct ClientHideScreenCommand : IBroadcast
    {
    }
}

public class FishNetWrapper
{
    private NetworkManager _network;
    private ServerManager _serverManager;
    private ClientManager _clientManager;

    public FishNetWrapper()
    {
        _network = InstanceFinder.NetworkManager;
        _serverManager = _network.ServerManager;
        _clientManager = _network.ClientManager;
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
}
