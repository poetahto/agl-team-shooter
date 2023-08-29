﻿using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class ServerTestGui : NetworkBehaviour
    {
        [SerializeField]
        private PlayerSpawner spawner;

        private void OnGUI()
        {
            foreach (var (player, body) in spawner.PlayersToBodies.Dictionary)
            {
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

                    if (IsServer)
                    {
                        if (GUILayout.Button("damage"))
                            livingEntity.ServerDamage(1);

                        if (GUILayout.Button("kill"))
                            livingEntity.ServerKill();
                    }
                }

                if (body.TryGetComponent(out ComeBackToLifeAfterDuration backToLife))
                    GUILayout.Label($"Respawn Time: {backToLife.syncedRemainingTime}");

                GUILayout.EndHorizontal();
            }
        }
    }
}
