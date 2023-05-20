using UnityEngine;

namespace Core
{
    public static class CommonTransitions
    {
        public static IVisibilityTransition Popup(CanvasGroup canvasGroup, float duration = 0.1f)
        {
            return new CompositeTransition(
                new CanvasGroupController(canvasGroup, duration),
                new SlideUpTransition(canvasGroup.transform, duration),
                new FadeTransition(canvasGroup, duration)
            );
        }
    }
}
