using UniRx;
using UnityEngine;

public interface IGameplayServices
{
    INetworkInterface NetworkInterface { get; }
    ILobby Lobby { get; }
    IGame Game { get; }
}

public interface ILobby
{
    void AddClient(IClient client);
    void RemoveClient(IClient client);
    IReadOnlyReactiveCollection<IClient> Clients { get; }
}

public class Lobby : ILobby
{
    private ReactiveCollection<IClient> _clients = new ReactiveCollection<IClient>();
    public IReadOnlyReactiveCollection<IClient> Clients => _clients;

    public void AddClient(IClient client)
    {
        _clients.Add(client);
    }

    public void RemoveClient(IClient client)
    {
        _clients.Add(client);
    }
}

public interface IGame
{
}

public interface IClient
{
    string Username { get; }
    int Id { get; }
}

public class Client : IClient
{
    public Client(string username, int id)
    {
        Username = username;
        Id = id;
    }

    public string Username { get; }
    public int Id { get; }
}

public class GameplayServices : MonoBehaviour, IGameplayServices
{
    public INetworkInterface NetworkInterface { get; } = new FishNetNetworkInterface();
    public ILobby Lobby { get; } = new Lobby();
    public IGame Game { get; } = new PayloadGame();
}
