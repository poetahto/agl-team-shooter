using System;
using FishNet.Object.Synchronizing;
using FSM;
using UniRx;
using UnityEngine;

namespace Gameplay.Modes.AttackDefend
{
    public class AttackPoint : GameplayNetworkBehavior
    {
        public enum State
        {
            Locked, // Cannot be attacked yet (out of order).
            Open, // Can be attacked.
            Captured, // Cannot be attacked, because it has been captured.

            // Sub-states of the open state.
            OpenCapturing, // The attackers are currently uncontested on the point.
            OpenContested, // There are both attackers and defenders on the point.
            OpenIdle, // There are no attackers on the point.
        }

        [SerializeField]
        private OpenState.Settings openStateSettings;

        [SerializeField] private Color gizmoColor = new Color(0, 1, 0, 0.25f);

        private StateMachine<State> _serverFsm;

        [NonSerialized]
        [SyncVar]
        public float SyncCapturePercent;

        [NonSerialized]
        [SyncVar]
        public int SyncAttackerCount;

        [NonSerialized]
        [SyncVar]
        public int SyncDefenderCount;

        [NonSerialized]
        [SyncVar(OnChange = nameof(HandleStateChange))]
        public State SyncState;

        [NonSerialized]
        [SyncVar]
        public State SyncOpenState;

        private OpenState _openState;
        private Subject<State> _stateChange;

        public IObservable<State> ObserveStateChanged() => _stateChange;

        protected override void Awake()
        {
            base.Awake();
            _stateChange = new Subject<State>();
        }

        public void Initialize()
        {
            _openState = new OpenState(this, openStateSettings);
            _serverFsm = new StateMachine<State>();
            _serverFsm.AddState(State.Locked, onExit: _ => SyncCapturePercent = 0);
            _serverFsm.AddState(State.Open, _openState);
            _serverFsm.AddState(State.Captured);
            _serverFsm.AddTriggerTransition("unlock", State.Locked, State.Open);
            _serverFsm.AddTransition(State.Open, State.Captured, _ => SyncCapturePercent >= 1);
            _serverFsm.SetStartState(State.Locked);
            _serverFsm.Init();
        }

        private void Update()
        {
            if (IsServer)
            {
                _serverFsm.OnLogic();
                SyncState = _serverFsm.ActiveState.name;

                if (SyncState == State.Open)
                    SyncOpenState = _openState.ActiveState.name;
            }
        }

        public void ServerUnlock()
        {
            _serverFsm.Trigger("unlock");
        }

        private void HandleStateChange(State previous, State current, bool asServer)
        {
            if (IsHost && !asServer)
                return;

            _stateChange.OnNext(current);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position + openStateSettings.triggerOffset, openStateSettings.triggerExtents * 2);
        }
    }
}
