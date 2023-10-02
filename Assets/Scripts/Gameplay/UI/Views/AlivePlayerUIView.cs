using FishNet.Object;
using UnityEngine;

namespace Gameplay
{
    public class AlivePlayerUIView : MonoUIView<NetworkObject>
    {
        [SerializeField] private LivingEntityUIView livingEntityUIView;

        public override void BindTo(NetworkObject instance)
        {
            if (instance.TryGetComponent(out LivingEntity livingEntity))
                livingEntityUIView.BindTo(livingEntity);
        }

        public override void ClearBindings()
        {
            livingEntityUIView.ClearBindings();
        }
    }
}
