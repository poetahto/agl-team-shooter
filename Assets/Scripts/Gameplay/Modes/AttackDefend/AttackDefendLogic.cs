using System;
using FishNet.Object;
using UniRx;
using UnityEngine;

namespace Gameplay.Modes.AttackDefend
{
    public class AttackDefendLogic : NetworkBehaviour
    {
        [Serializable]
        public struct Stage
        {
            public AttackPoint point;
            // todo: add custom stage logic (forward spawns, vfx, ect.)
        }

        [SerializeField]
        private Stage[] stages;

        private int _currentStage;
        private IDisposable _currentStageDisposable;

        public override void OnStartServer()
        {
            base.OnStartServer();

            foreach (Stage stage in stages)
                stage.point.Initialize();

            ApplyStage(0);
        }

        private void ApplyStage(int stageIndex)
        {
            if (stageIndex >= stages.Length)
            {
                print("attackers win!");
                // we are out of stages, the attackers win.
                // todo: attacker win logic, another state machine?
                return;
            }

            _currentStageDisposable?.Dispose();
            Stage stage = stages[stageIndex];

            _currentStageDisposable = stage.point.ObserveStateChanged()
                .Where(state => state == AttackPoint.State.Captured)
                .Subscribe(_ => AdvanceStage());

            stage.point.ServerUnlock();

            // todo: call into custom stage logic
        }

        private void AdvanceStage()
        {
            ApplyStage(++_currentStage);
        }
    }
}
