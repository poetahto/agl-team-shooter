﻿using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class GameplayNetworkBehavior : NetworkBehaviour
    {
        protected Lobby Lobby { get; private set; }
        protected PlayerSpawner PlayerSpawner { get; private set; }

        protected virtual void Awake()
        {
            Lobby = FindAnyObjectByType<Lobby>(FindObjectsInactive.Include);
            PlayerSpawner = FindAnyObjectByType<PlayerSpawner>(FindObjectsInactive.Include);
        }
    }
}
