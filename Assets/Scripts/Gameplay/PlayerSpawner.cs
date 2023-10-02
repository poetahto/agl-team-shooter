using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Manages the spawning of controllable player prefabs whenever their state
    /// changes. For instance, creates a respawning body when you die, and picks
    /// the appropriate loadout prefab when you respawn.
    /// </summary>
    public class PlayerSpawner : GameplayNetworkBehavior
    {
        [SerializeField]
        private LoadoutPrefabs loadoutPrefabs;

        [SerializeField]
        private NetworkObject respawningPrefab;

        [SyncObject]
        private readonly SyncDictionary<ConnectedPlayer, NetworkObject> _playersToBodies =
            new SyncDictionary<ConnectedPlayer, NetworkObject>();

        public SyncReactiveDictionary<ConnectedPlayer, NetworkObject> PlayersToBodies { get; private set; }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            PlayersToBodies = new SyncReactiveDictionary<ConnectedPlayer, NetworkObject>(_playersToBodies);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            Lobby.Players.ObserveAdd()
                .Subscribe(eventData => HandlePlayerJoin(eventData.Value))
                .AddTo(this);

            PlayersToBodies.ObserveRemove()
                .Subscribe(HandleBodyRemoved)
                .AddTo(this);

            PlayersToBodies.ObserveAdd()
                .Subscribe(HandleBodyAdded)
                .AddTo(this);
        }

        private void HandleBodyAdded(DictionaryAddEvent<ConnectedPlayer, NetworkObject> eventData)
        {
            Spawn(eventData.Value, eventData.Key.Owner);
        }

        private void HandleBodyRemoved(DictionaryRemoveEvent<ConnectedPlayer, NetworkObject> eventData)
        {
            Despawn(eventData.Value);
        }

        private void HandlePlayerJoin(ConnectedPlayer player)
        {
            player.ObservePlayerStateChanged()
                .DelayFrame(1)
                .Subscribe(_ => HandlePlayerStateChanged(player))
                .AddTo(player)
                .AddTo(this);

            HandlePlayerStateChanged(player);
        }

        private void HandlePlayerStateChanged(ConnectedPlayer player)
        {
            NetworkObject bodyPrefab = player.syncedPlayerState switch
            {
                PlayerState.Alive => loadoutPrefabs.GetLoadoutPrefab(player),
                PlayerState.Respawning => respawningPrefab,
                _ => throw new ArgumentOutOfRangeException(),
            };

            if (_playersToBodies.ContainsKey(player))
                _playersToBodies.Remove(player);

            Transform spawn = SpawnPoint.GetSpawn(player.syncedTeamId).transform;
            NetworkObject bodyInstance = Instantiate(bodyPrefab, spawn.position, spawn.rotation);
            _playersToBodies.Add(player, bodyInstance);
        }
    }
}
