using System;
using System.Collections;
using UniRx;

namespace Core
{
    public class CompositeTransition : VisibilityTransitionBase
    {
        private readonly IVisibilityTransition[] _transitions;
        private IDisposable _animation;

        public CompositeTransition(params IVisibilityTransition[] transitions) : base(false)
        {
            _transitions = transitions;
        }

        protected override IEnumerator ShowCoroutine()
        {
            foreach (var transition in _transitions)
                transition.Show();

            yield return Observable.FromCoroutine(() => WaitForAll(true)).ToYieldInstruction();
        }

        protected override IEnumerator HideCoroutine()
        {
            foreach (var transition in _transitions)
                transition.Hide();

            yield return Observable.FromCoroutine(() => WaitForAll(false)).ToYieldInstruction();
        }

        private IEnumerator WaitForAll(bool target)
        {
            while (true)
            {
                bool allDone = true;

                foreach (var transition in _transitions)
                {
                    if (transition.IsVisible != target)
                        allDone = false;
                }

                if (allDone)
                    break;

                yield return null;
            }
        }
    }
}
