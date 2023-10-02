using System;
using FishNet.Object;
using Gameplay.Modes.AttackDefend;
using UnityEngine;

namespace Gameplay
{
    public class ServerTestGui : NetworkBehaviour
    {
        [SerializeField]
        private PlayerSpawner spawner;

        private void OnGUI()
        {
            DrawPlayerData();
            DrawAttackPointData();
        }

        private void DrawAttackPointData()
        {
            foreach (AttackPoint attackPoint in FindObjectsByType<AttackPoint>(FindObjectsSortMode.None))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{attackPoint.name} [{attackPoint.syncState}]");

                switch (attackPoint.syncState)
                {
                    case AttackPoint.State.Open:
                        GUILayout.Label($"{attackPoint.syncOpenState} {attackPoint.syncCapturePercent:F3}: attackers={attackPoint.syncAttackerCount} defenders={attackPoint.syncDefenderCount}");
                        break;

                    case AttackPoint.State.Locked:
                        if (IsServer && GUILayout.Button("Unlock"))
                            attackPoint.ServerUnlock();
                        break;
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawPlayerData()
        {
            foreach (var (player, body) in spawner.PlayersToBodies.ReactiveDictionary)
            {
                GUILayout.BeginHorizontal();

                TeamDefinition team = player.GetTeam();
                GUILayout.Label($"{player.syncedPlayerName} {player.syncedPlayerState} {team.ColoredName} {player.syncedLoadout}");

                if (IsServer)
                {
                    if (GUILayout.Button("set alive"))
                        player.syncedPlayerState = PlayerState.Alive;

                    if (GUILayout.Button("set respawning"))
                        player.syncedPlayerState = PlayerState.Respawning;

                    if (GUILayout.Button("cycle team"))
                        player.syncedTeamId = (player.syncedTeamId + 1) % TeamDefinition.GetTeamCount();

                    if (GUILayout.Button("cycle loadout"))
                        player.syncedLoadout = (Loadout) (((int) player.syncedLoadout + 1) % Enum.GetValues(typeof(Loadout)).Length);
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
