using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class GameplayNetworkBehavior : NetworkBehaviour
    {
        public Lobby Lobby { get; private set; }
        public PlayerSpawner PlayerSpawner { get; private set; }

        protected virtual void Awake()
        {
            Lobby = FindAnyObjectByType<Lobby>(FindObjectsInactive.Include);
            PlayerSpawner = FindAnyObjectByType<PlayerSpawner>(FindObjectsInactive.Include);
        }
    }
}
