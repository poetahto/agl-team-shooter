using System;

namespace Gameplay
{
    public class RespawningState : ClientGameLogicState
    {
        [Serializable]
        public class Settings
        {
            
        }
        
        public RespawningState(ClientGameLogic parent, Settings settings) : base(parent)
        {
        }
    }
}