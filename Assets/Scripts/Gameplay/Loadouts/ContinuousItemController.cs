using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ContinuousItemController : MonoBehaviour, IInputItemController
    {
        [SerializeField]
        private bool cancelOnSwitch = true;

        [SerializeField]
        private UnityEvent onUseStart;

        [SerializeField]
        private UnityEvent onUseEnd;

        private bool _wasUsing;
        public bool IsUsing { get; private set; }

        public void OnClientStart()
        {
        }

        public void OnClientLogic()
        {
            IsUsing = Input.GetKey(KeyCode.Mouse0);

            if (!_wasUsing && IsUsing) // we just started
            {
                onUseStart.Invoke();
            }

            if (_wasUsing && !IsUsing) // we just stopped
                onUseEnd.Invoke();

            _wasUsing = IsUsing;
        }

        public void OnClientSelectStart()
        {
        }

        public void OnClientSelectStop()
        {
            if (IsUsing && cancelOnSwitch)
            {
                IsUsing = false;
                _wasUsing = false;
                onUseEnd.Invoke();
            }
        }
    }
}
