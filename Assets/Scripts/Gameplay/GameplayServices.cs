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
}

public interface IGame
{
}

public interface IMap
{
    string SceneName { get; }
}

public interface IClient
{
    string Username { get; }
}

public class GameplayServices : MonoBehaviour, IGameplayServices
{
    public INetworkInterface NetworkInterface { get; } = new FishNetNetworkInterface();
    public ILobby Lobby { get; set; }
    public IGame Game { get; set; } = new PayloadGame();
}
