using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class BoundedDiscreteResource : MonoBehaviour
    {
        [SerializeField]
        private int unitsPerRound = 10;

        [SerializeField]
        private int maxCapacity = 50;

        [SerializeField]
        private UnityEvent onUseSuccess = new UnityEvent();

        [SerializeField]
        private UnityEvent onUseFail = new UnityEvent();

        [SerializeField]
        private UnityEvent onRefill = new UnityEvent();

        [SerializeField]
        private UnityEvent onReload = new UnityEvent();

        public IntReactiveProperty CurrentAmount { get; private set; }
        public IntReactiveProperty RemainingAmount { get; private set; }

        public IObservable<Unit> ObserveUseSuccess() => onUseSuccess.AsObservable();
        public IObservable<Unit> ObserveUseFail() => onUseFail.AsObservable();
        public IObservable<Unit> ObserveRefill() => onRefill.AsObservable();
        public IObservable<Unit> ObserveReload() => onReload.AsObservable();

        private void Start()
        {
            CurrentAmount = new IntReactiveProperty(unitsPerRound);
            RemainingAmount = new IntReactiveProperty(maxCapacity);
        }

        public void Reload()
        {
            int reloadAmount = Mathf.Min(unitsPerRound - CurrentAmount.Value, RemainingAmount.Value);
            RemainingAmount.Value = Mathf.Max(RemainingAmount.Value - reloadAmount, 0);
            CurrentAmount.Value += reloadAmount;
            onRefill.Invoke();
        }

        public void Refill(int amount)
        {
            RemainingAmount.Value = Mathf.Min(RemainingAmount.Value + amount, maxCapacity);
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
