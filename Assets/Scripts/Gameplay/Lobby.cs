using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;

/// <summary>
/// Associates connections with players in the game.
/// </summary>
public class Lobby : NetworkBehaviour
{
    /// <summary>
    /// All of the players that are currently connected in this server.
    /// This list is accurate and synced across all clients, and the server.
    /// </summary>
    public SyncReactiveList<PlayerData> Players { get; private set; }

    // The internal list that actually synchronizes the data.
    [SyncObject]
    private readonly SyncList<PlayerData> _clients = new SyncList<PlayerData>();

    private Dictionary<NetworkConnection, PlayerData> _connectionsToClients;

    private void Awake()
    {
        Players = new SyncReactiveList<PlayerData>(_clients);
        _connectionsToClients = new Dictionary<NetworkConnection, PlayerData>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        ServerManager.OnRemoteConnectionState += Server_HandleRemoteConnectionState;

        if (IsHost)
        {
            Server_AddClient(ClientManager.Connection, new PlayerData{Username = "Host"});
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        ServerManager.OnRemoteConnectionState -= Server_HandleRemoteConnectionState;
    }

    [Server]
    private void Server_HandleRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        switch (args.ConnectionState)
        {
            case RemoteConnectionState.Started:
                Server_AddClient(connection, new PlayerData{Username = $"Player {args.ConnectionId}"});
                break;
            case RemoteConnectionState.Stopped:
                Server_RemoveClient(connection);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Server]
    private void Server_AddClient(NetworkConnection connection, PlayerData player)
    {
        _clients.Add(player);
        _connectionsToClients.Add(connection, player);
    }

    [Server]
    private void Server_RemoveClient(NetworkConnection connection)
    {
        _clients.Remove(_connectionsToClients[connection]);
        _connectionsToClients.Remove(connection);
    }
}
