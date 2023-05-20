using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using UnityEngine;

public interface INetworkInterface
{
    UniTask Host(ushort port);
    UniTask Connect(string address, ushort port);
    void LoadScene(string sceneName);
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

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneName){ReplaceScenes = ReplaceOption.All});
    }
}
