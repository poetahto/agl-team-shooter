using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BoundedDiscreteResource : MonoBehaviour
    {
        [SerializeField]
        private int initialCapacity = 50;

        [SerializeField]
        private int maxCapacity = 50;

        [SerializeField]
        private UnityEvent onUseSuccess = new UnityEvent();

        [SerializeField]
        private UnityEvent onUseFail = new UnityEvent();

        [SerializeField]
        private UnityEvent onRefill = new UnityEvent();

        public IntReactiveProperty CurrentAmount { get; private set; }

        public IObservable<Unit> ObserveUseSuccess() => onUseSuccess.AsObservable();
        public IObservable<Unit> ObserveUseFail() => onUseFail.AsObservable();
        public IObservable<Unit> ObserveRefill() => onRefill.AsObservable();

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
