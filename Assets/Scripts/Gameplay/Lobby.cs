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
