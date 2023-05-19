using UnityEngine;

public interface IGameplayServices
{
    INetworkInterface NetworkInterface { get; }
    IGameUI GameUI { get; }
}

public class GameplayServices : MonoBehaviour, IGameplayServices
{
    public INetworkInterface NetworkInterface { get; set; }
    public IGameUI GameUI { get; set; }
}
