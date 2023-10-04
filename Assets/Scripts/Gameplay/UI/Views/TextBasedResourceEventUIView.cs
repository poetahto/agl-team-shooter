using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.UI.Views
{
    public class TextBasedResourceEventUIView : MonoUIView<BoundedDiscreteResource>
    {
        [SerializeField]
        private TMP_Text currentText;

        [SerializeField]
        private TMP_Text remainingText;

        private IDisposable _disposable;

        public override void BindTo(BoundedDiscreteResource instance)
        {
            var compositeDisposable = new CompositeDisposable();

            compositeDisposable.Add(instance.CurrentAmount
                .Subscribe(eventData => currentText.SetText(eventData.ToString())));

            compositeDisposable.Add(instance.RemainingAmount
                .Subscribe(eventData => remainingText.SetText(eventData.ToString())));

            _disposable = compositeDisposable;
        }

        public override void ClearBindings()
        {
            _disposable?.Dispose();
        }
    }
}
