using DefaultNamespace;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class RocketLauncherItemController : InputItemController
    {
        [SerializeField]
        private BoundedDiscreteResource ammo;

        [SerializeField]
        private PredictedProjectileLauncher launcher;

        private void Start()
        {
            ammo.ObserveUseSuccess().Subscribe(_ => launcher.ClientFire());
        }

        public override void OnClientLogic()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                ammo.Use();

            if (Input.GetKeyDown(KeyCode.R))
                ammo.Refill();
        }
    }
}
