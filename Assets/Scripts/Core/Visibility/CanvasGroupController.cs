using System.Collections;
using UnityEngine;

namespace Core
{
    public class CanvasGroupController : VisibilityTransitionBase
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly float _duration;

        public CanvasGroupController(CanvasGroup canvasGroup, float duration, bool startVisible = false) : base(startVisible)
        {
            _canvasGroup = canvasGroup;
            _duration = duration;
            _canvasGroup.SetVisible(startVisible);
        }

        protected override IEnumerator ShowCoroutine()
        {
            _canvasGroup.EnableInteraction();
            yield return new WaitForSeconds(_duration);
        }

        protected override IEnumerator HideCoroutine()
        {
            yield return new WaitForSeconds(_duration);
            _canvasGroup.DisableInteraction();
        }
    }
}