using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SpawnPoint : MonoBehaviour
    {
        private static Dictionary<int, List<SpawnPoint>> _spawnPointLookup;
        private static int _currentSpawnIndex;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _spawnPointLookup = new Dictionary<int, List<SpawnPoint>>();
            _currentSpawnIndex = 0;
        }

        [SerializeField]
        private int teamId;

        private void Awake()
        {
            if (!_spawnPointLookup.TryGetValue(teamId, out List<SpawnPoint> spawnPoints))
            {
                spawnPoints = new List<SpawnPoint>();
                _spawnPointLookup.Add(teamId, spawnPoints);
            }

            spawnPoints.Add(this);
        }

        public static SpawnPoint GetSpawn(int teamId)
        {
            SpawnPoint result = _spawnPointLookup[teamId][_currentSpawnIndex];
            _currentSpawnIndex = (_currentSpawnIndex + 1) % _spawnPointLookup.Count;
            return result;
        }
    }
}
