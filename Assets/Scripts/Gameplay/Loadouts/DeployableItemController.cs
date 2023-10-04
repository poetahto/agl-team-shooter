using FishNet.Object;
using FSM;
using UnityEngine;

namespace Gameplay
{
    public class DeployableItemController : NetworkBehaviour, IInputItemController
    {
        public enum State
        {
            Idle,
            Planning,
        }

        [SerializeField]
        private GameObject deployablePreviewPrefab;

        [SerializeField]
        private NetworkObject deployablePrefab;

        [SerializeField]
        private Transform viewDirection;

        [SerializeField]
        private float deployRange = 5;

        [SerializeField]
        private float rotateSpeed = 1;

        private StateMachine<State> _fsm;

        public void OnClientStart()
        {
            _fsm = new StateMachine<State>();
            _fsm.AddState(State.Idle);
            _fsm.AddState(State.Planning, new PlanningState{Controller = this});
            _fsm.AddTransition(State.Idle, State.Planning, _ => Input.GetKeyDown(KeyCode.Mouse0));
            _fsm.AddTransition(State.Planning, State.Idle, _ => Input.GetKeyDown(KeyCode.Mouse1));
            _fsm.SetStartState(State.Idle);
            _fsm.Init();
        }

        public void OnClientLogic()
        {
            _fsm.OnLogic();
        }

        public void OnClientSelectStart()
        {
        }

        public void OnClientSelectStop()
        {
        }

        [ServerRpc(RequireOwnership = true)]
        private void Rpc_ServerSpawnDeployable(Vector3 position, Quaternion rotation)
        {
            NetworkObject instance = Instantiate(deployablePrefab, position, rotation);
            Spawn(instance, Owner);
        }

        private class PlanningState : State<State>
        {
            private const int BufferSize = 50;

            public DeployableItemController Controller;

            private bool _hasTarget;
            private Vector3 _targetPosition;
            private float _targetRotation;
            private GameObject _previewInstance;
            private RaycastHit[] _hitBuffer = new RaycastHit[BufferSize];
            private bool _waitFrame;

            public override void OnEnter()
            {
                base.OnEnter();
                _previewInstance = Instantiate(Controller.deployablePreviewPrefab);
                _waitFrame = true;
                _previewInstance.SetActive(false);
            }

            public override void OnLogic()
            {
                base.OnLogic();
                if (_waitFrame)
                {
                    _waitFrame = false;
                    return;
                }
                Ray ray = new Ray(Controller.viewDirection.position, Controller.viewDirection.forward);
                int hits = Physics.RaycastNonAlloc(ray, _hitBuffer, Controller.deployRange);

                if (_hitBuffer.TryGetNearest(out RaycastHit nearest, hits))
                {
                    float angle = Vector3.Angle(nearest.normal, Vector3.up);
                    _hasTarget = angle <= 45;
                    print(angle);
                    _targetPosition = nearest.point;
                }
                else _hasTarget = false;

                _previewInstance.transform.SetPositionAndRotation(_targetPosition, Quaternion.AngleAxis(_targetRotation, Vector3.up));
                _previewInstance.SetActive(_hasTarget);

                if (Input.GetKeyDown(KeyCode.Mouse0) && _hasTarget)
                {
                    Controller.Rpc_ServerSpawnDeployable(_targetPosition, Quaternion.AngleAxis(_targetRotation, Vector3.up));
                    fsm.RequestStateChange(State.Idle);
                }

                if (Input.GetKey(KeyCode.E))
                    _targetRotation += Controller.rotateSpeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.Q))
                    _targetRotation -= Controller.rotateSpeed * Time.deltaTime;
            }

            public override void OnExit()
            {
                base.OnExit();
                Destroy(_previewInstance);
            }
        }
    }
}
