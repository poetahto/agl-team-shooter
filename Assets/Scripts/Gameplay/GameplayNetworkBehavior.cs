using FishNet.Object;

namespace Gameplay
{
    public class GameplayNetworkBehavior : NetworkBehaviour
    {
        protected Lobby Lobby { get; private set; }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            Lobby = FindAnyObjectByType<Lobby>();
        }
    }
}
