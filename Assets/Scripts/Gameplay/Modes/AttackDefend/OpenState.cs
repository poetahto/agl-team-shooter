using System;
using System.Collections.Generic;
using Core;
using FishNet.Object;
using FSM;
using UnityEngine;

namespace Gameplay.Modes.AttackDefend
{
    public class OpenState : StateMachine<AttackPoint.State>
    {
        [Serializable]
        public class Settings
        {
            public int attackingTeam;
            public int defendingTeam;
            public float captureRate;
            public Lobby lobby; // todo: better way of referencing
            public Vector3 triggerOffset;
            public Vector3 triggerExtents;
        }

        private readonly AttackPoint _point;
        private readonly Settings _settings;
        private Collider[] _overlapBuffer;
        private List<int> _ownerIdBuffer;

        public OpenState(AttackPoint attackPoint, Settings settings, int bufferSize = 100)
        {
            _point = attackPoint;
            _settings = settings;
            _overlapBuffer = new Collider[bufferSize];
            _ownerIdBuffer = new List<int>();

            this.AddState(AttackPoint.State.OpenIdle);
            this.AddState(AttackPoint.State.OpenCapturing, onLogic: _ => attackPoint.syncCapturePercent += attackPoint.syncAttackerCount * settings.captureRate * Time.deltaTime);
            this.AddState(AttackPoint.State.OpenContested);
            this.AddTransitionFromAny(AttackPoint.State.OpenIdle, _ => attackPoint.syncAttackerCount == 0);
            this.AddTransitionFromAny(AttackPoint.State.OpenCapturing, _ => attackPoint.syncAttackerCount != 0 && attackPoint.syncDefenderCount == 0);
            this.AddTransitionFromAny(AttackPoint.State.OpenContested, _ => attackPoint.syncAttackerCount != 0 && attackPoint.syncDefenderCount != 0);

            SetStartState(AttackPoint.State.OpenIdle);
        }

        public override void OnLogic()
        {
            UpdateAttackerAndDefenderCount();
            base.OnLogic();
        }

        private void UpdateAttackerAndDefenderCount()
        {
            int hitCount = Physics.OverlapBoxNonAlloc(_point.transform.position + _settings.triggerOffset, _settings.triggerExtents, _overlapBuffer);
            _point.syncAttackerCount = 0;
            _point.syncDefenderCount = 0;
            _ownerIdBuffer.Clear();

            for (int i = 0; i < hitCount; i++)
            {
                Collider col = _overlapBuffer[i];

                if (col.TryGetComponentWithRigidbody(out NetworkObject networkObject)
                    && !_ownerIdBuffer.Contains(networkObject.OwnerId)
                    && _settings.lobby.TryFindPlayer(networkObject, out ConnectedPlayer player))
                {
                    _ownerIdBuffer.Add(networkObject.OwnerId);

                    if (player.syncedTeamId == _settings.attackingTeam)
                        _point.syncAttackerCount++;

                    else if (player.syncedTeamId == _settings.defendingTeam)
                        _point.syncDefenderCount++;
                }
            }
        }
    }
}
