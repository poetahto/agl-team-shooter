using System;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class PlayerSpawner : GameplayBehavior
    {
        [SerializeField]
        private NetworkObject playerPrefab;

        private Dictionary<PlayerData, NetworkObject> _playersToBodies;

        private void Awake()
        {
            _playersToBodies = new Dictionary<PlayerData, NetworkObject>();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            Lobby.Players.ObserveAdd()
                .Subscribe(addEvent => Server_HandlePlayerJoin(addEvent.Value))
                .AddTo(this);

            Lobby.Players.ObserveRemove()
                .Subscribe(removeEvent => Server_HandlePlayerLeave(removeEvent.Value))
                .AddTo(this);

            foreach (PlayerData player in Lobby.Players)
                Server_HandlePlayerJoin(player);
        }

        [Server]
        private void Server_HandlePlayerJoin(PlayerData player)
        {
            NetworkObject instance = Instantiate(playerPrefab, transform.position, transform.rotation);
            ServerManager.Spawn(instance, ServerManager.Clients[player.Id]);
            _playersToBodies.Add(player, instance);
        }

        [Server]
        private void Server_HandlePlayerLeave(PlayerData player)
        {
            ServerManager.Despawn(_playersToBodies[player]);
            _playersToBodies.Remove(player);
        }
    }
}
