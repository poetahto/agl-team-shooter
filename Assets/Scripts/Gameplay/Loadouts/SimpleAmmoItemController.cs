using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class SimpleAmmoItemController : InputItemController
    {
        [SerializeField]
        private BoundedDiscreteResource ammo;

        [SerializeField]
        private UnityEvent onClientFire;

        public override void OnClientStart()
        {
            base.OnClientStart();

            ammo.ObserveUseSuccess().Subscribe(_ => onClientFire.Invoke());
        }

        public override void OnClientLogic()
        {
            base.OnClientLogic();

            if (Input.GetKeyDown(KeyCode.Mouse0))
                ammo.Use();

            if (Input.GetKeyDown(KeyCode.R))
                ammo.Refill();
        }
    }
}
