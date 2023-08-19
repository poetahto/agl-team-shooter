using FishNet.Object;
using UnityEngine;

namespace Core.Networking
{
    public class OwnerOnly : NetworkBehaviour
    {
        [SerializeField] private GameObject[] gameObjectTargets;
        [SerializeField] private Behaviour[] behaviorTargets;

        public override void OnStartClient()
        {
            base.OnStartClient();

            foreach (var gameObjectTarget in gameObjectTargets)
                gameObjectTarget.SetActive(IsOwner);

            foreach (var behaviorTarget in behaviorTargets)
                behaviorTarget.enabled = IsOwner;
        }
    }
}
