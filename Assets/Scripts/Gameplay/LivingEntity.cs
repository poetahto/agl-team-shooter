using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;
using UnityEngine;

namespace Gameplay
{
    public class LivingEntity : NetworkBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;

        [SyncVar(OnChange = nameof(HandleMaxHealthChange))]
        public int syncedMaxHealth;

        [SyncVar(OnChange = nameof(HandleHealthChange))]
        public int syncedCurrentHealth;

        private Subject<HealthChangeEvent> _onHealthChange;
        private Subject<MaxHealthChangeEvent> _onMaxHealthChange;

        public IObservable<HealthChangeEvent> ObserveHealthChange() => _onHealthChange;
        public IObservable<MaxHealthChangeEvent> ObserveMaxHealthChange() => _onMaxHealthChange;

        public float PercentHealth => (float) syncedCurrentHealth / syncedMaxHealth;

        private void Awake()
        {
            _onHealthChange = new Subject<HealthChangeEvent>();
            _onMaxHealthChange = new Subject<MaxHealthChangeEvent>();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            syncedMaxHealth = maxHealth;
            syncedCurrentHealth = maxHealth;
        }

        [Server]
        public void ServerDamage(int amount)
        {
            syncedCurrentHealth -= amount;
        }

        private void HandleHealthChange(int previous, int next, bool asServer)
        {
            int healthChange = next - previous;

            if (healthChange != 0)
            {
                var healthChangeEvent = new HealthChangeEvent
                {
                    Entity = this,
                    PreviousHealth = previous,
                    CurrentHealth = next,
                };

                _onHealthChange.OnNext(healthChangeEvent);
            }
        }

        private void HandleMaxHealthChange(int previous, int next, bool asServer)
        {
            int maxHealthChange = next - previous;

            if (maxHealthChange != 0)
            {
                var maxHealthChangeEvent = new MaxHealthChangeEvent
                {
                    Entity = this,
                    PreviousMaxHealth = previous,
                    CurrentMaxHealth = next,
                };

                _onMaxHealthChange.OnNext(maxHealthChangeEvent);
            }
        }

        public struct MaxHealthChangeEvent
        {
            public LivingEntity Entity;
            public int PreviousMaxHealth;
            public int CurrentMaxHealth;
        }

        public struct HealthChangeEvent
        {
            public LivingEntity Entity;
            public int PreviousHealth;
            public int CurrentHealth;
        }
    }
}
