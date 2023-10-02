using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay
{
    public class LoadoutItemSystem : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> loadoutItems;

        private Subject<SelectedItemChangeEvent> _onSelectedItemChange = new Subject<SelectedItemChangeEvent>();

        public int SelectedIndex { get; private set; }
        public GameObject SelectedItem { get; private set; }
        public IReadOnlyList<GameObject> Items => loadoutItems;
        public IObservable<SelectedItemChangeEvent> ObserveSelectedItemChange() => _onSelectedItemChange;

        private void Awake()
        {
            Assert.IsTrue(loadoutItems.Count > 0);
            HandleSelectedIndex(0);
        }

        public void HandleSelectedIndex(int index)
        {
            SelectedItemChangeEvent eventData;
            eventData.oldIndex = SelectedIndex;
            eventData.oldItem = SelectedItem;

            SelectedIndex = index % loadoutItems.Count;
            SelectedItem = loadoutItems[SelectedIndex];

            eventData.newIndex = SelectedIndex;
            eventData.newItem = SelectedItem;
            _onSelectedItemChange.OnNext(eventData);
        }
    }

    public struct SelectedItemChangeEvent
    {
        public int newIndex;
        public int oldIndex;
        public GameObject newItem;
        public GameObject oldItem;
    }
}
