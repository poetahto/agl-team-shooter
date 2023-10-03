using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay
{
    // Note: full client authority / trust here, not a good place for it but easier than rollback raycasting.
    // Todo: bullet knockback + whatever extra effects
    public class HitscanLogic : GameplayNetworkBehavior
    {
        private const int BufferSize = 50;

        [SerializeField]
        private Transform viewTransform;

        [SerializeField]
        private int hitDamage = 5;

        private RaycastHit[] _hitBuffer = new RaycastHit[BufferSize];

        public void ClientFire()
        {
            Ray ray = new Ray(viewTransform.position, viewTransform.forward);
            int hits = Physics.RaycastNonAlloc(ray, _hitBuffer);
            Assert.IsTrue(hits <= BufferSize);

            if (_hitBuffer.TryGetNearest(out RaycastHit nearest, hits))
            {
                ConnectedPlayer player = Lobby.FindPlayer(Owner);
                Collider hitCollider = nearest.collider;

                if (hitCollider.TryGetConnectedPlayer(out ConnectedPlayer hitPlayer) && hitPlayer.ShouldTakeDamageFrom(player))
                    Rpc_ServerDamage(PlayerSpawner.PlayersToBodies.ReactiveDictionary[hitPlayer].ObjectId);
            }
        }

        [ServerRpc(RequireOwnership = true)]
        private void Rpc_ServerDamage(int objectId)
        {
            NetworkObject hitObject = InstanceFinder.ServerManager.Objects.Spawned[objectId];

            if (hitObject.TryGetComponent(out LivingEntity livingEntity))
                livingEntity.ServerDamage(hitDamage);
        }
    }
}
