using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UnityEngine;

public class Lobby : NetworkBehaviour, ILobby
{
    [SerializeField]
    private GameplayServices services;

    // The list that is actually synchronized between client/server
    [SyncObject]
    private readonly SyncList<Client> _syncedClients = new SyncList<Client>();

    // A user-friendly list that is easier to manage callbacks for in C# scripts, and free from FishNet.
    private ReactiveCollection<Client> _clients = new ReactiveCollection<Client>();
    public IReadOnlyReactiveCollection<Client> Clients => _clients;

    private void Awake()
    {
        _syncedClients.OnChange += HandleClientChange;
        services.Lobby = this;
    }

    [Server]
    public void AddClient(Client client)
    {
        _syncedClients.Add(client);
    }

    [Server]
    public void RemoveClient(Client client)
    {
        _syncedClients.Remove(client);
    }

    private void HandleClientChange(SyncListOperation operation, int index, Client _, Client client, bool asServer)
    {
        // Prevent the host from accidentally handling doubled messages.
        if (IsHost && asServer)
            return;

        switch (operation)
        {
            case SyncListOperation.Add:
                print("add");
                _clients.Add(client);
                break;
            case SyncListOperation.Insert:
                _clients.Insert(index, client);
                break;
            case SyncListOperation.Set:
                _clients[index] = client;
                break;
            case SyncListOperation.RemoveAt:
                _clients.RemoveAt(index);
                break;
            case SyncListOperation.Clear:
                _clients.Clear();
                break;
            case SyncListOperation.Complete:
                // Do nothing.
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
        }
    }
}
