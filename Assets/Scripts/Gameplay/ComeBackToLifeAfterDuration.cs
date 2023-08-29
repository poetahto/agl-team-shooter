using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Gameplay
{
    public class ComeBackToLifeAfterDuration : NetworkBehaviour
    {
        [SyncVar]
        public float syncedRemainingTime = 5;

        private bool _isDead;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _isDead = true;
        }

        private void Update()
        {
            if (IsServer)
            {
                syncedRemainingTime -= Time.deltaTime;

                if (_isDead && syncedRemainingTime <= 0)
                {
                    _isDead = false;
                    ConnectedPlayer player = ConnectedPlayer.GetPlayer(Owner);
                    player.syncedPlayerState = PlayerState.Alive;
                }
            }
        }
    }
}
