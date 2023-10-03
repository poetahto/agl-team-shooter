using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ContinuousItemController : InputItemController
    {
        [SerializeField] 
        private bool cancelOnSwitch = true;
        
        [SerializeField]
        private UnityEvent onUseStart;
        
        [SerializeField]
        private UnityEvent onUseEnd;

        private bool _wasUsing;
        public bool IsUsing { get; private set; }
        
        public override void OnClientLogic()
        {
            base.OnClientLogic();
            
            IsUsing = Input.GetKey(KeyCode.Mouse0);

            if (!_wasUsing && IsUsing) // we just started
            {
                onUseStart.Invoke();
            }

            if (_wasUsing && !IsUsing) // we just stopped
                onUseEnd.Invoke();
            
            _wasUsing = IsUsing;
        }

        public override void OnClientSelectStop()
        {
            base.OnClientSelectStop();

            if (IsUsing && cancelOnSwitch)
            {
                IsUsing = false;
                _wasUsing = false;
                onUseEnd.Invoke();
            }
        }
    }
}