using FSM;

namespace Gameplay
{
    public class ClientGameLogicState : State
    {
        protected ClientGameLogic Parent { get; }

        public ClientGameLogicState(ClientGameLogic parent)
        {
            Parent = parent;
        }
    }
}