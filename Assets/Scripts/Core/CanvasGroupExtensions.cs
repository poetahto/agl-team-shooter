using UnityEngine;

namespace Core
{
    public static class CanvasGroupExtensions
    {
        public static void DisableInteraction(this CanvasGroup canvasGroup)
        {
            SetInteraction(canvasGroup, false);
        }

        public static void EnableInteraction(this CanvasGroup canvasGroup)
        {
            SetInteraction(canvasGroup, true);
        }

        public static void Show(this CanvasGroup canvasGroup)
        {
            SetVisible(canvasGroup, true);
        }

        public static void Hide(this CanvasGroup canvasGroup)
        {
            SetVisible(canvasGroup, false);
        }

        public static void SetVisible(this CanvasGroup canvasGroup, bool visible)
        {
            canvasGroup.alpha = visible ? 1 : 0;
            SetInteraction(canvasGroup, visible);
        }

        public static void SetInteraction(this CanvasGroup canvasGroup, bool isInteractable)
        {
            canvasGroup.interactable = isInteractable;
            canvasGroup.blocksRaycasts = isInteractable;
        }
    }
}
