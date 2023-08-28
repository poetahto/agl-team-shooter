using TMPro;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class LivingEntityUIView : MonoUIView<LivingEntity>
    {
        [SerializeField]
        private TMP_Text healthText;

        [SerializeField]
        private Gradient healthColorGradient;

        private CompositeDisposable _bindings = new CompositeDisposable();

        public override void BindTo(LivingEntity instance)
        {
            _bindings.Add(instance.ObserveHealthChange().Subscribe(eventData => UpdateHealthView(eventData.Entity)).AddTo(this));
            _bindings.Add(instance.ObserveMaxHealthChange().Subscribe(eventData => UpdateHealthView(eventData.Entity)).AddTo(this));
        }

        private void UpdateHealthView(LivingEntity entity)
        {
            healthText.text = entity.CurrentHealth.ToString();
            healthText.color = healthColorGradient.Evaluate(entity.PercentHealth);
        }

        public override void ClearBindings()
        {
            _bindings?.Dispose();
            _bindings?.Clear();
        }
    }
}
