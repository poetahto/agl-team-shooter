using Core;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class RocketLogic : GameplayMonoBehavior
    {
        [SerializeField] private Explosion explosion;

        private void OnTriggerEnter(Collider other)
        {
            var proj = GetComponent<PredictedProjectile>();
            ConnectedPlayer player = Lobby.FindPlayer(proj.OwnerObject);

            if (other.TryGetComponentWithRigidbody(out NetworkObject hitObject))
            {
                ConnectedPlayer hitPlayer = Lobby.FindPlayer(hitObject);

                if (hitPlayer != null && (hitPlayer != player || hitPlayer.syncedTeamId != player.syncedTeamId))
                    SpawnExplosion(proj.OwnerObject);
            }
            else if (!other.isTrigger)
                SpawnExplosion(proj.OwnerObject);
        }

        private void SpawnExplosion(NetworkObject owner)
        {
            Explosion explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstance.Play(owner);
            Destroy(gameObject);
        }
    }
}
