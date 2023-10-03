using FishNet.Object;
using FSM;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay
{
    // Again, client authoritative healing here. Not as big a deal as damage, but still consideration.
    public class HealingBeamLogic : GameplayNetworkBehavior
    {
        private const int BufferSize = 50;

        public enum State
        {
            Idle,
            Healing,
        }

        [SerializeField]
        private float healthPerSecond = 10;

        [SerializeField]
        private int healTicksPerSecond = 10;

        [SerializeField]
        private Transform viewDirection;

        private RaycastHit[] _hitBuffer = new RaycastHit[BufferSize];
        private StateMachine<State> _fsm;
        private NetworkObject _healingTarget;
        private float _elapsed;
        private float HealCooldown => (float) 1/ healTicksPerSecond;
        private int HealthPerTick => (int) (healthPerSecond / healTicksPerSecond);

        public override void OnStartServer()
        {
            base.OnStartServer();
            _fsm = new StateMachine<State>();

            _fsm.AddState(State.Idle, onEnter: _ => _healingTarget = null);
            _fsm.AddState(State.Healing, onLogic: _ => ApplyHealing());

            _fsm.AddTransition(State.Healing, State.Idle, _ => _healingTarget == null);
            _fsm.AddTransition(State.Idle, State.Healing, _ => _healingTarget != null);

            _fsm.SetStartState(State.Idle);
            _fsm.Init();
        }

        private void ApplyHealing()
        {
            if (_healingTarget.TryGetComponent(out LivingEntity livingEntity))
            {
                if (_elapsed >= HealCooldown)
                {
                    _elapsed = 0;
                    livingEntity.ServerHeal(HealthPerTick);
                }

                _elapsed += Time.deltaTime;
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                _fsm.OnLogic();
                Debug.Log($"{_fsm.ActiveStateName} {(_healingTarget ? _healingTarget.name : "no target")}");
            }
        }

        public void ClientStartHeal()
        {
            // physics cast to try and find someone you can heal, assign it to healing target
            Ray ray = new Ray(viewDirection.position, viewDirection.forward);
            int hits = Physics.RaycastNonAlloc(ray, _hitBuffer);
            Assert.IsTrue(hits <= BufferSize);
            ConnectedPlayer player = Lobby.FindPlayer(Owner);

            if (_hitBuffer.TryGetNearest(out RaycastHit nearest, hits) && nearest.collider.TryGetConnectedPlayer(out ConnectedPlayer hitPlayer) && hitPlayer.ShouldTakeHealFrom(player))
                Rpc_ServerStartHeal(PlayerSpawner.PlayersToBodies.ReactiveDictionary[hitPlayer].ObjectId);
        }

        public void ClientEndHeal()
        {
            Rpc_ServerEndHeal();
        }

        [ServerRpc(RequireOwnership = true)]
        private void Rpc_ServerStartHeal(int objectId)
        {
            _healingTarget = ServerManager.Objects.Spawned[objectId];
        }

        [ServerRpc(RequireOwnership = true)]
        private void Rpc_ServerEndHeal()
        {
            _healingTarget = null;
        }
    }
}
