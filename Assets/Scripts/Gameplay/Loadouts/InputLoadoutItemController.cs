using Core;
using UnityEngine;

namespace Gameplay
{
    public class InputLoadoutItemController : MonoBehaviour
    {
        [SerializeField]
        private KeyCode primaryKey = KeyCode.Mouse0;

        [SerializeField]
        private KeyCode secondaryKey = KeyCode.Mouse1;

        [SerializeField]
        private KeyCode reloadKey = KeyCode.R;

        [SerializeField]
        private LoadoutItemSystem itemSystem;

        private void Update()
        {
            if (InputUtil.TryGetAlphaKeyDown(out int key))
                itemSystem.HandleSelectedIndex(key - 1);

            if (Input.GetKeyDown(primaryKey) && itemSystem.SelectedItem.TryGetComponent(out PrimaryEffect primaryEffect))
                primaryEffect.Play();

            if (Input.GetKeyDown(secondaryKey) && itemSystem.SelectedItem.TryGetComponent(out SecondaryEffect secondaryEffect))
                secondaryEffect.Play();

            if (Input.GetKeyDown(reloadKey) && itemSystem.SelectedItem.TryGetComponent(out ReloadEffect reloadEffect))
                reloadEffect.Play();
        }
    }
}
