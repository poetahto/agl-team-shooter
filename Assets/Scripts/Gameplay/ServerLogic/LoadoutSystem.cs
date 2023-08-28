using System;
using System.Collections.Generic;
using Application.Gameplay.Dialogue;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class LoadoutSystem : NetworkBehaviour
    {
        public enum Loadout
        {
            Testing,
        }

        [SerializeField]
        private Loadout defaultLoadout = Loadout.Testing;

        [SerializeField]
        private DictionaryGenerator<Loadout, NetworkObject> loadoutPrefabGenerator;

        private Dictionary<NetworkConnection, Loadout> _connectionsToLoadouts;
        private Dictionary<Loadout, NetworkObject> _loadoutPrefabs;

        public override void OnStartServer()
        {
            base.OnStartServer();

            _connectionsToLoadouts = new Dictionary<NetworkConnection, Loadout>();
            _loadoutPrefabs = loadoutPrefabGenerator.GenerateDictionary();
        }

        [Server]
        public void ServerHandlePlayerJoin(NetworkConnection connection)
        {
            _connectionsToLoadouts.Add(connection, defaultLoadout);
        }

        [Server]
        public void ServerHandlePlayerLeave(NetworkConnection connection)
        {
            _connectionsToLoadouts.Remove(connection);
        }

        [Server]
        public void ServerSetConnectionLoadout(NetworkConnection connection, Loadout loadout)
        {
            _connectionsToLoadouts[connection] = loadout;
        }

        [Server]
        public NetworkObject ServerGetLoadoutPrefab(Loadout loadout)
        {
            return _loadoutPrefabs[loadout];
        }

        [Server]
        public NetworkObject ServerGetLoadoutPrefab(NetworkConnection connection)
        {
            Loadout loadout = _connectionsToLoadouts[connection];
            return _loadoutPrefabs[loadout];
        }
    }
}
