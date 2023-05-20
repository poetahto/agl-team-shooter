using UnityEngine;

namespace Core
{
    public static class CommonTransitions
    {
        public static IVisibilityTransition Popup(CanvasGroup canvasGroup, Transform transform, float duration = 0.1f)
        {
            return new CompositeTransition(
                new CanvasGroupController(canvasGroup, duration),
                new FadeTransition(canvasGroup, duration),
                new ScaleTransition(transform, duration)
            );
        }

        public static IVisibilityTransition SlideAndFade(CanvasGroup canvasGroup, Transform transform, float duration = 0.1f)
        {
            return new CompositeTransition(
                new CanvasGroupController(canvasGroup, duration),
                new FadeTransition(canvasGroup, duration),
                new SlideUpTransition(transform, duration)
            );
        }

        public static IVisibilityTransition Fade(CanvasGroup canvasGroup, float duration = 0.1f)
        {
            return new CompositeTransition(
                new CanvasGroupController(canvasGroup, duration),
                new FadeTransition(canvasGroup, duration)
            );
        }
    }
}
