using FishNet;
using FishNet.Managing.Scened;
using UnityEngine;

public class PayloadGame : IGame
{
    public void LoadMap(IPayloadMap map)
    {
        // Load default map when game starts.
        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(map.SceneName));
    }
}

public interface IPayloadMap
{
    string SceneName { get; }
}
