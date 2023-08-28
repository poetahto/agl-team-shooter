using System;

namespace Gameplay
{
    [Serializable]
    public class AliveState : PlayerGameLogicState
    {
        public AlivePlayerUIView alivePlayerUI;

        public override void OnEnter(bool asServer)
        {
            if (Parent.Owner.IsLocalClient)
                alivePlayerUI.BindTo(Parent.NetworkObject);
        }

        public override void OnExit(bool asServer)
        {
            if (Parent.Owner.IsLocalClient)
                alivePlayerUI.ClearBindings();
        }
    }
}
