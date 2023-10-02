using Core;
using UnityEngine;

namespace Gameplay
{
    public class InputLoadoutItemController : MonoBehaviour
    {
        [SerializeField]
        private LoadoutItemSystem itemSystem;

        private void Update()
        {
            if (InputUtil.TryGetAlphaKeyDown(out int key))
                itemSystem.HandleSelectedIndex(key - 1);

            if (Input.GetKeyDown(KeyCode.Mouse0) && itemSystem.SelectedItem.TryGetComponent(out PrimaryEffect primaryEffect))
                primaryEffect.Play();

            if (Input.GetKeyDown(KeyCode.Mouse1) && itemSystem.SelectedItem.TryGetComponent(out SecondaryEffect secondaryEffect))
                secondaryEffect.Play();
        }
    }
}
