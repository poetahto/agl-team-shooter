using UnityEngine;
using UnityEngine.UI;

namespace Poetools.UI
{
    /// <summary>
    /// Automatically applies graphic settings to a list of graphics.
    /// </summary>
    public class GraphicApplier : MonoBehaviour
    {
        [SerializeField] private Graphic[] targets;
        public GraphicAuthoring authoring;

        private void Awake()
        {
            foreach (var target in targets)
                target.color = authoring.Color;
        }
    }
}
