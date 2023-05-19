using FishNet;
using UnityEngine;

public interface INetworkInterface
{
    void Host(ushort port);
    void Connect(string address, ushort port);
}

[RequireComponent(typeof(GameplayServices))]
public class FishNetNetworkInterface : MonoBehaviour, INetworkInterface
{
    private void Awake()
    {
        GetComponent<GameplayServices>().NetworkInterface = this;
    }

    public void Host(ushort port)
    {
        InstanceFinder.ServerManager.StartConnection(port);
    }

    public void Connect(string address, ushort port)
    {
        InstanceFinder.ClientManager.StartConnection(address, port);
    }
}
