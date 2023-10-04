using FishNet.Object;
using UnityEngine;

namespace Gameplay.Console
{
    public class HealthPickupLogic : MonoBehaviour
    {
        [SerializeField]
        private int healAmount = 20;

        public void HandleHealthPickup(NetworkObject target)
        {
            if (target.TryGetComponent(out LivingEntity livingEntity))
                livingEntity.ServerHeal(healAmount);
        }
    }
}
