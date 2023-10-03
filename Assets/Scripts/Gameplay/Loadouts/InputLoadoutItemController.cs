using Core;
using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class InputLoadoutItemController : NetworkBehaviour
    {
        [SerializeField]
        private LoadoutItemSystem itemSystem;

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (IsOwner)
            {
                foreach (var item in itemSystem.Items)
                {
                    if (item.TryGetComponent(out InputItemController itemController))
                        itemController.OnClientStart();
                }
            }
        }

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
