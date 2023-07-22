using Poetools.UI.Items;
using UnityEngine;

namespace Poetools.UI
{
    /// <summary>
    /// A collection of menu item logic that is commonly found in games, and
    /// usually depends entirely upon core Unity features.
    /// </summary>
    public static class CommonMenuItems
    {
        /// <summary>
        /// A dropdown menu that displays and update the current screen resolution,
        /// found in the <see cref="Screen"/>
        /// </summary>
        /// <returns></returns>
        public static IMenuItem ResolutionDropdown()
        {
            return Dropdown.FromOptions(
                Screen.resolutions,
                resolution => $"{resolution.width}x{resolution.height} ({resolution.refreshRateRatio} HZ)",
                resolution => () => Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio)
            );
        }

        /// <summary>
        /// A dropdown menu that displays and updates the current Quality Level,
        /// found in the <see cref="QualitySettings"/>.
        /// </summary>
        /// <returns></returns>
        public static IMenuItem QualityDropdown()
        {
            return Dropdown.FromOptions(QualitySettings.names, index => QualitySettings.SetQualityLevel(index, true));
        }

        /// <summary>
        /// A toggle for enabling and disabling fullscreen mode, found
        /// in <see cref="Screen"/>.
        /// </summary>
        /// <returns></returns>
        public static IMenuItem FullscreenToggle()
        {
            return new Toggle(changed: isOn => Screen.fullScreen = isOn);
        }

        /// <summary>
        /// A button for closing the application, and will
        /// also exit play mode if running in the editor.
        /// </summary>
        /// <param name="title">The text to display directly on the button.</param>
        /// <returns></returns>
        public static IMenuItem QuitGameButton(string title = "Quit")
        {
            return new Button(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }, title);
        }
    }
}
