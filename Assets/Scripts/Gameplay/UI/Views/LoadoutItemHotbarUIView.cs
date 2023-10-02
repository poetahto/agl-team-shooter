using UnityEngine;

namespace Gameplay
{
    public class LoadoutItemHotbarUIView : MonoUIView<LoadoutItemSystem>
    {
        [SerializeField]
        private Transform hotbarLayoutGroup;

        public override void ClearBindings()
        {
            for (int i = hotbarLayoutGroup.childCount - 1; i >= 0; i--)
            {
                Destroy(hotbarLayoutGroup.GetChild(i).gameObject);
            }
        }

        public override void BindTo(LoadoutItemSystem instance)
        {
            foreach (GameObject item in instance.Items)
            {
                if (item.TryGetComponent(out LoadoutItemUIData uiData))
                {
                    uiData.UIView.transform.SetParent(hotbarLayoutGroup);
                }
            }
        }
    }
}
