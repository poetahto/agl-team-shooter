using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class ServerTestGui : NetworkBehaviour
    {
        [SerializeField]
        private PlayerSpawner spawner;

        private void OnGUI()
        {
            foreach (KeyValuePair<ConnectedPlayer, NetworkObject> kvp in spawner.PlayersToBodies.Dictionary)
            {
                ConnectedPlayer player = kvp.Key;
                NetworkObject body = kvp.Value;

                GUILayout.BeginHorizontal();

                GUILayout.Label($"{player.syncedPlayerName} {player.syncedPlayerState} {player.syncedTeamId} {player.syncedLoadout}");

                if (IsServer)
                {
                    if (GUILayout.Button("set alive"))
                        player.syncedPlayerState = PlayerState.Alive;

                    if (GUILayout.Button("set respawning"))
                        player.syncedPlayerState = PlayerState.Respawning;
                }

                if (body.TryGetComponent(out LivingEntity livingEntity))
                {
                    GUILayout.Label($"[health {livingEntity.syncedCurrentHealth}/{livingEntity.syncedMaxHealth}]");

                    if (IsServer && GUILayout.Button("damage"))
                        livingEntity.ServerDamage(1);
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}
