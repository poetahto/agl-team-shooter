namespace Gameplay
{
    public interface IInputItemController
    {
        public void OnClientStart();
        public void OnClientLogic();
        public void OnClientSelectStart();
        public void OnClientSelectStop();
    }
}
