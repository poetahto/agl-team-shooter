using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class LivingEntityServerTester : NetworkBehaviour
    {
        private void OnGUI()
        {
            foreach (var livingEntity in FindObjectsByType<LivingEntity>(FindObjectsSortMode.None))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{livingEntity.ObjectId}: {livingEntity.syncedCurrentHealth}/{livingEntity.syncedMaxHealth}");
                if (IsServer && GUILayout.Button("damage")) livingEntity.ServerDamage(1);
                GUILayout.EndHorizontal();
            }
        }
    }
}
