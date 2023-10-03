using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class SimpleAmmoItemController : MonoBehaviour, IInputItemController
    {
        [SerializeField]
        private BoundedDiscreteResource ammo;

        [SerializeField]
        private UnityEvent onClientFire;

        public void OnClientStart()
        {
            ammo.ObserveUseSuccess().Subscribe(_ => onClientFire.Invoke());
        }

        public void OnClientLogic()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                ammo.Use();

            if (Input.GetKeyDown(KeyCode.R))
                ammo.Refill();
        }

        public void OnClientSelectStart()
        {
        }

        public void OnClientSelectStop()
        {
        }
    }
}
