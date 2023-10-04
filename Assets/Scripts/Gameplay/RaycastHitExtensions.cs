using Core;
using FishNet.Object;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay
{
    public static class RaycastHitExtensions
    {
        public static bool TryGetNearest(this RaycastHit[] hits, out RaycastHit nearest, int length = -1)
        {
            nearest = default;

            if ((length < 0 && hits.Length <= 0) || length == 0)
                return false;

            if (length <= 0)
                length = hits.Length;

            nearest = hits[0];

            for (int i = 0; i < length; i++)
            {
                if (hits[i].distance < nearest.distance)
                    nearest = hits[i];
            }

            return true;
        }

        public static bool ShouldTakeDamageFrom(this ConnectedPlayer self, ConnectedPlayer player)
        {
            return self == player || self.syncedTeamId != player.syncedTeamId;
        }

        public static bool ShouldTakeHealFrom(this ConnectedPlayer self, ConnectedPlayer player)
        {
            return self.syncedTeamId == player.syncedTeamId;
        }

        public static bool TryGetConnectedPlayer(this Collider collider, out ConnectedPlayer player)
        {
            player = null;
            Lobby lobby = Object.FindAnyObjectByType<Lobby>();
            return collider.TryGetComponentWithRigidbody(out NetworkObject hitObject) && lobby.TryFindPlayer(hitObject, out player);
        }

        public static bool TryGetConnectedPlayerBody(this Collider collider, out NetworkObject body)
        {
            PlayerSpawner spawner = Object.FindAnyObjectByType<PlayerSpawner>();

            if (collider.TryGetConnectedPlayer(out ConnectedPlayer connectedPlayer))
            {
                return spawner.PlayersToBodies.ReactiveDictionary.TryGetValue(connectedPlayer, out body);
            }

            body = null;
            return false;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T value)
        {
            value = gameObject.GetComponentInChildren<T>();
            return value != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component component, out T value)
        {
            value = component.GetComponentInChildren<T>();
            return value != null;
        }

    }
}
