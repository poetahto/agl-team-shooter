using Core;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class InputLoadoutItemController : NetworkBehaviour
    {
        [SerializeField]
        private LoadoutItemSystem itemSystem;

        private void Update()
        {
            if (IsOwner)
            {
                if (InputUtil.TryGetAlphaKeyDown(out int key))
                    itemSystem.HandleSelectedIndex(key - 1);

                if (itemSystem.SelectedItem.TryGetComponent(out InputItemController itemController))
                    itemController.OnClientLogic();
            }
        }
    }
}
