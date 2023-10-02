using System.Collections.Generic;
using Application.Gameplay.Combat;
using Core;
using FishNet;
using FishNet.Object;
using poetools.Core.Abstraction;
using UnityEngine;

namespace Gameplay
{
    public class Explosion : GameplayMonoBehavior
    {
        [SerializeField]
        private int damage = 25;

        [SerializeField]
        private float knockback = 5;

        [SerializeField]
        private float knockbackUpBoost = 2;

        [SerializeField]
        private float radius = 3;

        public void Play(NetworkObject owner)
        {
            var damagedObjects = new List<NetworkObject>();
            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            var player = Lobby.FindPlayer(owner);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponentWithRigidbody(out NetworkObject hitObject))
                {
                    var hitPlayer = Lobby.FindPlayer(hitObject);

                    if (!damagedObjects.Contains(hitObject) && hitPlayer != null && (player == hitPlayer || hitPlayer.syncedTeamId != player.syncedTeamId))
                    {
                        ApplyExplosion(hit);
                        damagedObjects.Add(hitObject);
                    }
                }
                else if (!hit.isTrigger)
                    ApplyExplosion(hit);
            }
        }

        private void ApplyExplosion(Collider col)
        {
            GameObject root = col.GetRootGameObject();

            if (InstanceFinder.IsServer)
            {
                if (root.TryGetComponent(out LivingEntity livingEntity))
                    livingEntity.ServerDamage(damage);

                if (root.TryGetComponent(out PhysicsComponent physics))
                {
                    Vector3 explosionVelocity = ProjectileMotion.GetExplosionVelocity(transform.position, root.transform.position, knockback, knockbackUpBoost);
                    physics.Velocity = explosionVelocity;
                }
            }
        }
    }
}
