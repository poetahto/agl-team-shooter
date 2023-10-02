using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

/// <summary>
/// Associates connections with players in the game.
/// </summary>
public class Lobby : NetworkBehaviour
{
    [SerializeField]
    private ConnectedPlayer playerPrefab;

    /// <summary>
    /// All of the players that are currently connected in this server.
    /// This list is accurate and synced across all clients, and the server.
    /// </summary>
    public SyncReactiveList<ConnectedPlayer> Players { get; private set; }

    // The internal list that actually synchronizes the data.
    [SyncObject]
    private readonly SyncList<ConnectedPlayer> _clients = new SyncList<ConnectedPlayer>();

    private void Awake()
    {
        Players = new SyncReactiveList<ConnectedPlayer>(_clients);
    }

    public ConnectedPlayer FindPlayer(NetworkConnection connection)
    {
        foreach (var player in Players)
        {
            if (player.Owner == connection)
                return player;
        }

        return null;
    }

    public ConnectedPlayer FindPlayer(NetworkObject networkObject)
    {
        return FindPlayer(networkObject.Owner);
    }

    public ConnectedPlayer FindLocalPlayer()
    {
        return FindPlayer(InstanceFinder.ClientManager.Connection);
    }

    public bool TryFindPlayer(NetworkConnection connection, out ConnectedPlayer result)
    {
        result = FindPlayer(connection);
        return result != null;
    }

    public bool TryFindPlayer(NetworkObject networkObject, out ConnectedPlayer result)
    {
        return TryFindPlayer(networkObject.Owner, out result);
    }

    public bool PlayerExists(NetworkConnection connection)
    {
        foreach (var player in Players)
        {
            if (player.Owner == connection)
                return true;
        }

        return false;
    }

    public bool PlayerExists(NetworkObject networkObject)
    {
        return PlayerExists(networkObject.Owner);
    }

    [Server]
    public void ServerHandlePlayerJoin(NetworkConnection connection)
    {
        var playerInstance = Instantiate(playerPrefab);
        Spawn(playerInstance.NetworkObject, connection);
        _clients.Add(playerInstance);
    }

    [Server]
    public void ServerHandlePlayerLeave(NetworkConnection connection)
    {
        var playerInstance = FindPlayer(connection);
        _clients.Remove(playerInstance);
    }
}
