using Core;
using FishNet.Object;
using UniRx;
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
                itemSystem.ObserveSelectedItemChange().Subscribe(eventData =>
                {
                    if (eventData.oldItem.TryGetComponent(out IInputItemController oldItemController))
                        oldItemController.OnClientSelectStop();

                    if (eventData.newItem.TryGetComponent(out IInputItemController newItemController))
                        newItemController.OnClientSelectStart();
                });

                foreach (var item in itemSystem.Items)
                {
                    if (item.TryGetComponent(out IInputItemController itemController))
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

                if (itemSystem.SelectedItem.TryGetComponent(out IInputItemController itemController))
                    itemController.OnClientLogic();
            }
        }
    }
}
