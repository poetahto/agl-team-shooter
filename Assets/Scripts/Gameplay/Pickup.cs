using FishNet.Object;
using FSM;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    // todo: people can collect stuff even if they dont need it (full health -> wasted hp, ect.)
    public class Pickup : GameplayNetworkBehavior
    {
        public enum State
        {
            Respawning,
            Active,
        }

        [SerializeField]
        private float respawnTime = 1;

        [SerializeField]
        private UnityEvent<NetworkObject> onCollect;

        [SerializeField] private GameObject view;

        private StateMachine<State> _fsm;
        private float _remainingRespawnTime;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _fsm = new StateMachine<State>();
            _fsm.AddState(State.Active, onEnter: _ => view.SetActive(true));

            _fsm.AddState(State.Respawning,
                onEnter: _ =>
                {
                    view.SetActive(false);
                    _remainingRespawnTime = respawnTime;
                },
                onLogic: _ => _remainingRespawnTime = Mathf.Max(_remainingRespawnTime - Time.deltaTime, 0));

            _fsm.AddTransition(State.Respawning, State.Active, _ => _remainingRespawnTime == 0);
            _fsm.SetStartState(State.Active);
            _fsm.Init();
        }

        private void Update()
        {
            if (IsServer)
                _fsm.OnLogic();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsServer && _fsm.ActiveState.name == State.Active && other.TryGetConnectedPlayerBody(out NetworkObject body))
            {
                onCollect.Invoke(body);
                _fsm.RequestStateChange(State.Respawning);
            }
        }
    }
}
