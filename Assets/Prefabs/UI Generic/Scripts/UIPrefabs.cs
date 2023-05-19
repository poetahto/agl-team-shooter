using UnityEngine;

namespace Poetools.UI
{
    /// <summary>
    /// A helper class for automatically setting up the UIPrefabs singleton,
    /// loaded from the Resources folder.
    /// </summary>
    internal class UIPrefabSingletonLoader
    {
        [RuntimeInitializeOnLoadMethod]
        private static void SetupUIPrefabs()
        {
            UIPrefabs uiPrefabs = Resources.Load<UIPrefabs>("UIPrefabs");
            UIPrefabs.Instance = uiPrefabs;
        }
    }

    /// <summary>
    /// A bridge between prefabs, stored in the asset database, and C# scripts at runtime.
    /// </summary>
    [CreateAssetMenu(menuName = "Custom/UI Prefabs", fileName = "UIPrefabs", order = 0)]
    public class UIPrefabs : ScriptableObject
    {
        /// <summary>
        /// Gets or sets a global instance of this object.
        /// </summary>
        public static UIPrefabs Instance { get; internal set; }

        // Every prefab that can be spawned from a MenuItem is bound here in the inspector.
        public GameObject button;
        public GameObject text;
        public GameObject toggle;
        public GameObject stringInput;
        public GameObject slider;
        public GameObject dropdown;
    }
}
