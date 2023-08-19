using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Broadcast;
using FishNet.Managing;
using FishNet.Managing.Scened;
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