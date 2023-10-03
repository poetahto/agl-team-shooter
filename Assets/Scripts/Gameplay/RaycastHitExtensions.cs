using Core;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public static class RaycastHitExtensions
    {
        public static bool ShouldTakeDamageFrom(this Collider collider, ConnectedPlayer player, Lobby lobby)
        {
            if (collider.TryGetComponentWithRigidbody(out NetworkObject hitObject))
            {
                var hitPlayer = lobby.FindPlayer(hitObject);

                if (hitPlayer != null && (player == hitPlayer || hitPlayer.syncedTeamId != player.syncedTeamId))
                {
                    return true;
                }
            }
            else if (!collider.isTrigger)
                return true;

            return false;
        }
    }
}
