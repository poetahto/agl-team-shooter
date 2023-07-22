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
    void AddClient(Client client);
    void RemoveClient(Client client);
    IReadOnlyReactiveCollection<Client> Clients { get; }
}

public interface IGame
{
}

public class Client
{
    public string Username;
    public int Id;
}

public class GameplayServices : MonoBehaviour, IGameplayServices
{
    public INetworkInterface NetworkInterface { get; } = new FishNetNetworkInterface();
    public ILobby Lobby { get; set; }
    public IGame Game { get; } = new PayloadGame();
}
