using Core;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class RocketLogic : MonoBehaviour
    {
        [SerializeField] private Explosion explosion;

        private void OnTriggerEnter(Collider other)
        {
            var proj = GetComponent<PredictedProjectile>();
            ConnectedPlayer player = ConnectedPlayer.GetPlayer(proj.Owner);

            if (other.TryGetComponentWithRigidbody(out NetworkObject hitObject))
            {
                ConnectedPlayer hitPlayer = ConnectedPlayer.GetPlayer(hitObject);

                if (hitPlayer != null && (hitPlayer != player || hitPlayer.syncedTeamId != player.syncedTeamId))
                    SpawnExplosion(proj.Owner);
            }
            else if (!other.isTrigger)
                SpawnExplosion(proj.Owner);
        }

        private void SpawnExplosion(NetworkObject owner)
        {
            print("spawned expl");
            var explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstance.Play(owner);
            Destroy(gameObject);
        }
    }
}
