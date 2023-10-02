using System;
using System.Collections.Generic;
using Application.Gameplay.Combat;
using Core;
using FishNet;
using FishNet.Object;
using poetools.Core.Abstraction;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay
{
    public class Explosion : GameplayMonoBehavior
    {
        private const int BufferSize = 50;

        [SerializeField]
        private int damage = 25;

        [SerializeField]
        private float knockback = 5;

        [SerializeField]
        private float knockbackUpBoost = 2;

        [SerializeField]
        private float radius = 3;

        // todo: pool?
        [SerializeField]
        private float lifetime = 1f;

        private Collider[] _hitBuffer = new Collider[BufferSize];
        private float _elapsedTime;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, radius);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > lifetime)
                Destroy(gameObject);
        }

        public void Play(NetworkObject owner)
        {
            var damagedObjects = new List<NetworkObject>();
            int hits = Physics.OverlapSphereNonAlloc(transform.position, radius, _hitBuffer);
            Assert.IsTrue(hits <= BufferSize);
            var player = Lobby.FindPlayer(owner);

            for (int i = 0; i < hits; i++)
            {
                var hit = _hitBuffer[i];

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
