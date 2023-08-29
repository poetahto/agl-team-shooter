using FishNet.Object;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class RespawnOnDeath : NetworkBehaviour
    {
        [SerializeField] private LivingEntity livingEntity;

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            livingEntity.ObserveDeath()
                .Subscribe(HandleOnDeath)
                .AddTo(this);
        }

        [Server]
        private void HandleOnDeath(LivingEntity.DeathEvent eventData)
        {
            ConnectedPlayer player = ConnectedPlayer.GetPlayer(Owner);
            player.syncedPlayerState = PlayerState.Respawning;
        }
    }
}