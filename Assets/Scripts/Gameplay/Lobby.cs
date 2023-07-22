using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;

public class ClientData
{
    public string Username;
}

public class Lobby : NetworkBehaviour
{
    // The list that is actually synchronized between client/server
    [SyncObject]
    private readonly SyncList<ClientData> _clients = new SyncList<ClientData>();

    private Dictionary<NetworkConnection, ClientData> _connectionsToClients
        = new Dictionary<NetworkConnection, ClientData>();

    public SyncReactiveList<ClientData> Clients { get; private set; }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        print("init lobby");
        Clients = new SyncReactiveList<ClientData>(_clients);

        if (IsServer)
        {
            ServerManager.OnRemoteConnectionState += Server_HandleRemoteConnectionState;
        }

        if (IsHost)
        {
            Server_AddClient(ClientManager.Connection, new ClientData{Username = "Host"});
        }
    }

    [Server]
    private void Server_AddClient(NetworkConnection connection, ClientData client)
    {
        _clients.Add(client);
        _connectionsToClients.Add(connection, client);
    }

    [Server]
    private void Server_RemoveClient(NetworkConnection connection)
    {
        _clients.Remove(_connectionsToClients[connection]);
        _connectionsToClients.Remove(connection);
    }

    [Server]
    private void Server_HandleRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        // Don't allow duplicate connections from the host.
        if (connection == InstanceFinder.ClientManager.Connection)
            return;

        switch (args.ConnectionState)
        {
            case RemoteConnectionState.Started:
                print("connected");
                Server_AddClient(connection, new ClientData{Username = $"Player {args.ConnectionId}"});
                break;
            case RemoteConnectionState.Stopped:
                print("left");
                Server_RemoveClient(connection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
