using System.Collections.Generic;
using Application.Gameplay.Dialogue;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu]
    public class LoadoutPrefabs : ScriptableObject
    {
        [SerializeField]
        private DictionaryGenerator<Loadout, NetworkObject> loadoutPrefabGenerator;

        private Dictionary<Loadout, NetworkObject> _loadoutPrefabs;

        private void OnValidate()
        {
            _loadoutPrefabs = loadoutPrefabGenerator.GenerateDictionary();
        }

        private void Awake()
        {
            _loadoutPrefabs = loadoutPrefabGenerator.GenerateDictionary();
        }

        public NetworkObject GetLoadoutPrefab(Loadout loadout)
        {
            return _loadoutPrefabs[loadout];
        }

        public NetworkObject GetLoadoutPrefab(ConnectedPlayer player)
        {
            return _loadoutPrefabs[player.syncedLoadout];
        }
    }
}
