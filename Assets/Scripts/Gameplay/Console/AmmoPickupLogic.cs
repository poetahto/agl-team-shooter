using FishNet.Object;
using UnityEngine;

namespace Gameplay.Console
{
    public class AmmoPickupLogic : MonoBehaviour
    {
        [SerializeField]
        private int ammoAmount = 20;

        public void HandleAmmoPickup(NetworkObject target)
        {
            if (target.TryGetComponentInChildren(out LoadoutItemSystem itemSystem))
            {
                if (itemSystem.SelectedItem.TryGetComponent(out BoundedDiscreteResource ammoResource))
                    ammoResource.Refill(ammoAmount);
            }
        }
    }
}
