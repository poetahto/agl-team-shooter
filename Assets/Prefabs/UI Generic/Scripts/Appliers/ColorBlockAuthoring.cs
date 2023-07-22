using UnityEngine;
using UnityEngine.UI;

namespace Poetools.UI
{
    /// <summary>
    /// SO wrapper for a color block (the pressed/highlighted/selected settings on a lot of UI elements).
    /// This is so you don't have to copy-paste settings around by hand, used in conjunction with <see cref="ColorBlockApplier"/>
    /// </summary>
    [CreateAssetMenu(menuName = "Custom/Color Block Authoring")]
    public class ColorBlockAuthoring : ScriptableObject
    {
        [SerializeField]
        private ColorBlock colorBlock;

        public ColorBlock ColorBlock => colorBlock;
    }
}
