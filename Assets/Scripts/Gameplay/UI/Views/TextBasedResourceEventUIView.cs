using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Gameplay.UI.Views
{
    public class TextBasedResourceEventUIView : MonoUIView<BoundedDiscreteResource>
    {
        [SerializeField]
        private TMP_Text text;

        private IDisposable _disposable;

        public override void BindTo(BoundedDiscreteResource instance)
        {
            _disposable = instance.CurrentAmount
                .Subscribe(eventData => text.SetText(eventData.ToString()));
        }

        public override void ClearBindings()
        {
            _disposable?.Dispose();
        }
    }
}
