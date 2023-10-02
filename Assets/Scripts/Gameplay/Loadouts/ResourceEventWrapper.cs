using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class ResourceEventWrapper : MonoBehaviour
    {
        [SerializeField]
        private int initialCapacity = 50;

        [SerializeField]
        private int maxCapacity = 50;

        [SerializeField]
        private UnityEvent onUseSuccess;

        [SerializeField]
        private UnityEvent onUseFail;

        [SerializeField]
        private UnityEvent onRefill;

        public IntReactiveProperty CurrentAmount { get; private set; }

        private void Start()
        {
            CurrentAmount = new IntReactiveProperty(initialCapacity);
        }

        public void Refill()
        {
            CurrentAmount.Value = maxCapacity;
            onRefill.Invoke();
        }

        public void Use()
        {
            if (CurrentAmount.Value > 0)
            {
                CurrentAmount.Value--;
                onUseSuccess.Invoke();
            }
            else
            {
                onUseFail.Invoke();
            }
        }
    }
}
