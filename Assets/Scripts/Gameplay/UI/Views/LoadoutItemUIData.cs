using UnityEngine;

namespace Gameplay
{
    public class LoadoutItemUIData : MonoBehaviour
    {
        [SerializeField]
        private GameObject uiView;

        public GameObject UIView => uiView;
    }
}
