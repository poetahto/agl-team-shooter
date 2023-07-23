using FishNet.Object;

namespace Gameplay
{
    public class GameplayBehavior : NetworkBehaviour
    {
        protected Lobby Lobby { get; private set; }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Lobby = FindObjectOfType<Lobby>();
        }
    }
}