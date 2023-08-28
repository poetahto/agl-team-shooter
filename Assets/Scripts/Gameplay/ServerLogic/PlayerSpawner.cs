using System;
using FishNet.Object;
using UniRx;
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

        public override void OnStartServer()
        {
            base.OnStartServer();

            foreach (ConnectedPlayer player in lobby.Players)
                HandlePlayerJoin(player);

            lobby.Players.ObserveAdd()
                .Subscribe(eventData => HandlePlayerJoin(eventData.Value))
                .AddTo(this);
        }

        private void HandlePlayerJoin(ConnectedPlayer player)
        {
            player.ObservePlayerStateChanged()
                .Subscribe(_ => HandlePlayerStateChanged(player))
                .AddTo(player)
                .AddTo(this);

            HandlePlayerStateChanged(player);
        }

        private void HandlePlayerStateChanged(ConnectedPlayer player)
        {
            NetworkObject loadoutPrefab = player.syncedPlayerState switch
            {
                PlayerState.Alive => loadoutPrefabs.GetLoadoutPrefab(player), // todo: destroy respawning prefab?
                PlayerState.Respawning => respawningPrefab, // todo: destroy alive prefab?
                _ => throw new ArgumentOutOfRangeException(),
            };
            NetworkObject loadoutInstance = Instantiate(loadoutPrefab); // todo: spawn points based on team
            Spawn(loadoutInstance, player.Owner);
        }
    }
}
