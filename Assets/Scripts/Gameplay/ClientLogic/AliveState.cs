using System;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class AliveState : ClientGameLogicState
    {
        private readonly Settings _settings;

        public AliveState(ClientGameLogic parent, Settings settings) : base(parent)
        {
            _settings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NetworkObject playerInstance = InstanceFinder.ClientManager.Connection.FirstObject;

            if (playerInstance != null)
            {
                _settings.alivePlayerUI.BindTo(playerInstance);
            }
            else Debug.LogError("Client has no body object!");
        }

        public override void OnExit()
        {
            base.OnExit();
            _settings.alivePlayerUI.ClearBindings();
        }

        [Serializable]
        public class Settings
        {
            [FormerlySerializedAs("ui")] public AlivePlayerUIView alivePlayerUI;
        }
    }
}
