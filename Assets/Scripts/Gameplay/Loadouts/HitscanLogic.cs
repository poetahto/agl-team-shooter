using System;
using System.Collections.Generic;
using Core;
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
        private Comparer<RaycastHit> _hitDistanceComparer = Comparer<RaycastHit>.Create((hitA, hitB) => hitA.distance.CompareTo(hitB.distance));

        public void ClientFire()
        {
            Ray ray = new Ray(viewTransform.position, viewTransform.forward);
            int hits = Physics.RaycastNonAlloc(ray, _hitBuffer);

            if (hits > 0)
            {
                Assert.IsTrue(hits <= BufferSize);
                ConnectedPlayer player = Lobby.FindPlayer(Owner);
                Array.Sort(_hitBuffer, 0, hits, _hitDistanceComparer);
                Collider hitCollider = _hitBuffer[0].collider;

                if (hitCollider.ShouldTakeDamageFrom(player, Lobby))
                {
                    if (hitCollider.TryGetComponentWithRigidbody(out NetworkObject networkObject))
                        Rpc_ServerDamage(networkObject.ObjectId);
                }
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
