using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Gameplay
{
    public class PlayerGameLogic : NetworkBehaviour
    {
        public enum State
        {
            Alive,
            Respawning,
        }

        [SerializeField]
        private AliveState aliveState;

        [SerializeField]
        private RespawningState respawningState;

        [SyncVar(OnChange = nameof(HandleStateChange))]
        public State syncedState = State.Alive;

        private PlayerGameLogicState _currentState;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            aliveState.Init(this);
            respawningState.Init(this);

            _currentState = syncedState switch
            {
                State.Alive => aliveState,
                State.Respawning => respawningState,
                _ => null,
            };

            if (IsServer) _currentState?.OnEnter(true);
            if (IsClient) _currentState?.OnEnter(false);
        }

        private void OnDestroy()
        {
            if (IsServer) _currentState?.OnExit(true);
            if (IsClient) _currentState?.OnExit(false);
        }

        private void Update()
        {
            if (IsServer) _currentState?.OnLogic(true);
            if (IsClient) _currentState?.OnLogic(false);
        }

        private void HandleStateChange(State previous, State current, bool asServer)
        {
            _currentState?.OnExit(asServer);
            _currentState = current switch
            {
                State.Alive => aliveState,
                State.Respawning => respawningState,
                _ => null,
            };
            _currentState?.OnEnter(asServer);
        }
    }
}
