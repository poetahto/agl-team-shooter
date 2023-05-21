using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public interface INetworkInterface
{
    UniTask<IClient> Host(ushort port);
    UniTask<IClient> Connect(string address, ushort port);
    UniTask LoadScene(string sceneName);
    void Disconnect();
}

public class FishNetNetworkInterface : INetworkInterface
{
    public async UniTask<IClient> Host(ushort port)
    {
        Debug.Log($"Hosting game on {port}");
        InstanceFinder.ServerManager.StartConnection(port);
        await UniTask.WaitUntil(() => InstanceFinder.IsServer);
        InstanceFinder.ClientManager.StartConnection();
        await UniTask.WaitUntil(() => InstanceFinder.IsClient);
        Debug.Log(InstanceFinder.IsHost);
        return new Client("Guest", InstanceFinder.ClientManager.Connection.ClientId);
    }

    public async UniTask<IClient> Connect(string address, ushort port)
    {
        Debug.Log($"Connecting to game on {address}:{port}");
        InstanceFinder.ClientManager.StartConnection(address, port);
        await UniTask.WaitUntil(() => InstanceFinder.IsClient);
        return new Client("Guest", InstanceFinder.ClientManager.Connection.ClientId);
    }

    public void Disconnect()
    {
        InstanceFinder.ClientManager.StopConnection();
    }

    public async UniTask LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        IDisposable loadingScreen = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneName){ReplaceScenes = ReplaceOption.All});
        loadingScreen.Dispose();
    }
}
