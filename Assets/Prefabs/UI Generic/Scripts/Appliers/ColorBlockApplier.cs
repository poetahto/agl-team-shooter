using UnityEngine;
using UnityEngine.UI;

namespace Poetools.UI
{
    /// <summary>
    /// Automatically applies color block settings to a list of selectables.
    /// </summary>
    public class ColorBlockApplier : MonoBehaviour
    {
        public ColorBlockAuthoring authoring;
        public Selectable[] targets;

        private void UpdateTargets()
        {
            foreach (var target in targets)
                target.colors = authoring.ColorBlock;
        }

        private void Awake()
        {
            UpdateTargets();
        }

#if UNITY_EDITOR
        private void Update()
        {
            // This is so we can adjust stuff in real-time and see them apply.
            // I had this in OnValidate before, but I think it was being weird.
            UpdateTargets();
        }
#endif
    }
}
