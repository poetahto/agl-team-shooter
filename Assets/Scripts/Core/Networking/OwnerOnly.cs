using FishNet.Object;
using UnityEngine;

namespace Core.Networking
{
    public class OwnerOnly : NetworkBehaviour
    {
        [SerializeField] private GameObject[] targets;

        public override void OnStartClient()
        {
            base.OnStartClient();

            foreach (var target in targets)
            {
                target.SetActive(IsOwner);
            }
        }
    }
}
