using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FSM;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Modes.AttackDefend
{
    public class AttackPoint : NetworkBehaviour
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

        [SyncVar]
        public float syncCapturePercent;

        [SyncVar]
        public int syncAttackerCount;

        [SyncVar]
        public int syncDefenderCount;

        [SyncVar(OnChange = nameof(HandleStateChange))]
        public State syncState;

        [SyncVar]
        public State syncOpenState;

        private OpenState _openState;
        private Subject<State> _stateChange;

        public IObservable<State> ObserveStateChanged() => _stateChange;

        private void Awake()
        {
            _stateChange = new Subject<State>();
        }

        // todo: this is jank way to fix timing issues w/ attackDefendLogic - get a better non hacky fix
        public void Initialize()
        {
            _openState = new OpenState(this, openStateSettings);
            _serverFsm = new StateMachine<State>();
            _serverFsm.AddState(State.Locked, onExit: _ => syncCapturePercent = 0);
            _serverFsm.AddState(State.Open, _openState);
            _serverFsm.AddState(State.Captured);
            _serverFsm.AddTriggerTransition("unlock", State.Locked, State.Open);
            _serverFsm.AddTransition(State.Open, State.Captured, _ => syncCapturePercent >= 1);
            _serverFsm.SetStartState(State.Locked);
            _serverFsm.Init();
        }

        private void Update()
        {
            if (IsServer)
            {
                _serverFsm.OnLogic();
                syncState = _serverFsm.ActiveState.name;

                if (syncState == State.Open)
                    syncOpenState = _openState.ActiveState.name;
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
