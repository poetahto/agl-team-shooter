using System;
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

        private IDisposable _bindings;

        public override void BindTo(LivingEntity instance)
        {
            healthText.enabled = true;

            var healthChange = instance.ObserveHealthChanged().Subscribe(eventData => UpdateHealthView(eventData.Entity));
            var maxHealthChange = instance.ObserveMaxHealthChanged().Subscribe(eventData => UpdateHealthView(eventData.Entity));
            _bindings = StableCompositeDisposable.Create(healthChange, maxHealthChange);
        }

        private void UpdateHealthView(LivingEntity entity)
        {
            healthText.text = entity.syncedCurrentHealth.ToString();
            healthText.color = healthColorGradient.Evaluate(entity.PercentHealth);
        }

        public override void ClearBindings()
        {
            healthText.enabled = false;
            _bindings?.Dispose();
        }
    }
}
