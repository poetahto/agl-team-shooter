using System;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField]
        private LoadoutPrefabs loadoutPrefabs;

        [SerializeField]
        private NetworkObject respawningPrefab;

        [SerializeField]
        private Lobby lobby;

        [SyncObject]
        private readonly SyncDictionary<ConnectedPlayer, NetworkObject> _playersToBodies =
            new SyncDictionary<ConnectedPlayer, NetworkObject>();

        public SyncReactiveDictionary<ConnectedPlayer, NetworkObject> PlayersToBodies { get; private set; }

        private void Awake()
        {
            PlayersToBodies = new SyncReactiveDictionary<ConnectedPlayer, NetworkObject>(_playersToBodies);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            lobby.Players.ObserveAdd()
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

            NetworkObject bodyInstance = Instantiate(bodyPrefab); // todo: spawn points based on team
            _playersToBodies.Add(player, bodyInstance);
        }
    }
}
