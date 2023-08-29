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
        private Subject<DeathEvent> _onDeath;

        public IObservable<HealthChangeEvent> ObserveHealthChanged() => _onHealthChange;
        public IObservable<MaxHealthChangeEvent> ObserveMaxHealthChanged() => _onMaxHealthChange;
        public IObservable<DeathEvent> ObserveDeath() => _onDeath;

        public float PercentHealth => (float) syncedCurrentHealth / syncedMaxHealth;

        private void Awake()
        {
            _onHealthChange = new Subject<HealthChangeEvent>();
            _onMaxHealthChange = new Subject<MaxHealthChangeEvent>();
            _onDeath = new Subject<DeathEvent>();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            syncedMaxHealth = maxHealth;
            syncedCurrentHealth = maxHealth;
        }

        [Server]
        public void ServerKill()
        {
            ServerDamage(syncedCurrentHealth);
        }

        [Server]
        public void ServerDamage(int amount)
        {
            int finalHealth = syncedCurrentHealth - amount;

            if (finalHealth <= 0 && syncedCurrentHealth > 0)
            {
                _onDeath.OnNext(new DeathEvent{Entity = this});
                Rpc_ObserverDeath();
            }

            syncedCurrentHealth = finalHealth;
        }

        [ObserversRpc(ExcludeServer = true)]
        private void Rpc_ObserverDeath()
        {
            _onDeath.OnNext(new DeathEvent{Entity = this});
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

        public struct DeathEvent
        {
            public LivingEntity Entity;
        }
    }
}
