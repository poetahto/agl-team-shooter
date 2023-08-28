namespace Gameplay
{
    public class PlayerGameLogicState
    {
        protected PlayerGameLogic Parent { get; private set; }

        public void Init(PlayerGameLogic parent)
        {
            Parent = parent;
        }

        public virtual void OnEnter(bool asServer) {}
        public virtual void OnExit(bool asServer) {}
        public virtual void OnLogic(bool asServer) {}
    }
}
