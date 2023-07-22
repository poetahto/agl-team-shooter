using System;
using Cysharp.Threading.Tasks;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;
using Channel = FishNet.Transporting.Channel;

public interface INetworkInterface
{
    UniTask<Client> Host(ushort port);
    UniTask<Client> Connect(string address, ushort port);
    UniTask LoadScene(string sceneName);
    void Spawn(GameObject instance, Client owner = null);
    void Disconnect();
    event Action<Client> ClientSpawned;
}

public class FishNetNetworkInterface : INetworkInterface
{
    struct ClientConnectedBroadcast : IBroadcast
    {
        public Client Client;
    }

    public async UniTask<Client> Host(ushort port)
    {
        Debug.Log($"Hosting game on {port}");
        InstanceFinder.ServerManager.StartConnection(port);
        await UniTask.WaitUntil(() => InstanceFinder.IsServer);
        InstanceFinder.ClientManager.StartConnection();
        await UniTask.WaitUntil(() => InstanceFinder.IsClient);
        Debug.Log(InstanceFinder.IsHost);
        InstanceFinder.ServerManager.RegisterBroadcast<ClientConnectedBroadcast>(ServerHandleClientConnected);
        var result = new Client{Username = "Guest", Id = InstanceFinder.ClientManager.Connection.ClientId};
        InstanceFinder.ClientManager.Broadcast(new ClientConnectedBroadcast{Client = result});
        return result;
    }

    private void ServerHandleClientConnected(NetworkConnection connection, ClientConnectedBroadcast data)
    {
        if (InstanceFinder.IsServerOnly)
            ClientSpawned?.Invoke(data.Client);

        InstanceFinder.ServerManager.Broadcast(data);
    }

    private void ClientHandleClientConnected(ClientConnectedBroadcast data)
    {
        ClientSpawned?.Invoke(data.Client);
    }

    public async UniTask<Client> Connect(string address, ushort port)
    {
        Debug.Log($"Connecting to game on {address}:{port}");
        InstanceFinder.ClientManager.StartConnection(address, port);
        await UniTask.WaitUntil(() => InstanceFinder.IsClient);
        InstanceFinder.ClientManager.RegisterBroadcast<ClientConnectedBroadcast>(ClientHandleClientConnected);
        var result = new Client{Username = "Guest", Id = InstanceFinder.ClientManager.Connection.ClientId};
        InstanceFinder.ClientManager.Broadcast(new ClientConnectedBroadcast{Client = result});
        return result;
    }

    [Server]
    public void Spawn(GameObject instance, Client owner = null)
    {
        InstanceFinder.ServerManager.Spawn(instance, owner == null ? null : InstanceFinder.ServerManager.Clients[owner.Id]);
    }

    public void Disconnect()
    {
        InstanceFinder.ClientManager.StopConnection();
    }

    public event Action<Client> ClientSpawned;

    public async UniTask LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene {sceneName}");
        IDisposable loadingScreen = await Services.LoadingScreenFactory.SlideRightColor(Color.black);
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneName){ReplaceScenes = ReplaceOption.All});
        loadingScreen.Dispose();
    }
}
