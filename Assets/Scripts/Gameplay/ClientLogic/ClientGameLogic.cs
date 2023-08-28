using FishNet.Object;
using FSM;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay
{
    public class ClientGameLogic : NetworkBehaviour
    {
        [SerializeField]
        private AliveState.Settings aliveSettings;

        [SerializeField]
        private RespawningState.Settings respawningSettings;

        private StateMachine _teamPlayerFsm;

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (ClientManager.Connection.FirstObject == null)
                ClientManager.Connection.OnObjectAdded += SetupStateMachine;

            else SetupStateMachine(ClientManager.Connection.FirstObject);
        }

        private void SetupStateMachine(NetworkObject obj)
        {
            ClientManager.Connection.OnObjectAdded -= SetupStateMachine;

            _teamPlayerFsm = new StateMachine();
            _teamPlayerFsm.AddState("alive", new AliveState(this, aliveSettings));
            _teamPlayerFsm.AddState("respawning", new RespawningState(this, respawningSettings));
            _teamPlayerFsm.SetStartState("alive");
            _teamPlayerFsm.Init();

            this.UpdateAsObservable()
                .Subscribe(_ => _teamPlayerFsm.OnLogic())
                .AddTo(this);
        }
    }
}
