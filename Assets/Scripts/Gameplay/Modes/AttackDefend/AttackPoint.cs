using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FSM;
using UnityEngine;

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

        [SyncVar]
        public State syncCurrentState;

        [SyncVar]
        public State syncOpenState;

        private OpenState _openState;

        public override void OnStartServer()
        {
            base.OnStartServer();
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
                syncCurrentState = _serverFsm.ActiveState.name;

                if (syncCurrentState == State.Open)
                    syncOpenState = _openState.ActiveState.name;
            }
        }

        [Server]
        public void ServerUnlock()
        {
            _serverFsm.Trigger("unlock");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position + openStateSettings.triggerOffset, openStateSettings.triggerExtents * 2);
        }
    }
}
