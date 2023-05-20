using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public interface INetworkInterface
{
    UniTask Host(ushort port);
    UniTask Connect(string address, ushort port);
    UniTask LoadScene(string sceneName);
    void Disconnect();
}

public class FishNetNetworkInterface : INetworkInterface
{
    public async UniTask Host(ushort port)
    {
        Debug.Log($"Hosting game on {port}");
        InstanceFinder.ServerManager.StartConnection(port);
        await UniTask.WaitUntil(() => InstanceFinder.ServerManager.Started);
    }

    public async UniTask Connect(string address, ushort port)
    {
        Debug.Log($"Connecting to game on {address}:{port}");
        InstanceFinder.ClientManager.StartConnection(address, port);
        await UniTask.WaitUntil(() => InstanceFinder.ClientManager.Started);
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
