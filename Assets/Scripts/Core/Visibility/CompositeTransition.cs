using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Core
{
    public class CompositeTransition : VisibilityTransitionBase
    {
        private readonly List<IVisibilityTransition> _showTransitions;
        private readonly List<IVisibilityTransition> _hideTransitions;
        private IDisposable _animation;

        public CompositeTransition(params IVisibilityTransition[] transitions) : base(false)
        {
            _showTransitions = new List<IVisibilityTransition>(transitions);
            _hideTransitions = new List<IVisibilityTransition>(transitions);
        }

        public CompositeTransition(IEnumerable<IVisibilityTransition> showTransitions, IEnumerable<IVisibilityTransition> hideTransitions) : base(false)
        {
            _showTransitions = new List<IVisibilityTransition>(showTransitions);
            _hideTransitions = new List<IVisibilityTransition>(hideTransitions);
        }

        protected override IEnumerator ShowCoroutine()
        {
            foreach (var transition in _showTransitions)
                transition.Show();

            yield return Observable.FromCoroutine(() => WaitForAll(true, _showTransitions)).ToYieldInstruction();
        }

        protected override IEnumerator HideCoroutine()
        {
            foreach (var transition in _hideTransitions)
                transition.Hide();

            yield return Observable.FromCoroutine(() => WaitForAll(false, _hideTransitions)).ToYieldInstruction();
        }

        private IEnumerator WaitForAll(bool target, List<IVisibilityTransition> transitions)
        {
            while (true)
            {
                bool allDone = true;

                foreach (var transition in transitions)
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
