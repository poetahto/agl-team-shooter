using System.Collections.Generic;
using Core;
using FishNet.Object;
using Gameplay.UI;
using TMPro;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreboardMenuLogic : NetworkBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private ConnectedPlayerListUIView playerListViewPrefab;

        [SerializeField]
        private Transform contentParent;

        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private Lobby lobby;

        private IVisibilityTransition _visibility;
        private bool _shown;

        private Dictionary<ConnectedPlayer, ConnectedPlayerListUIView> _viewLookup
            = new Dictionary<ConnectedPlayer, ConnectedPlayerListUIView>();

        private void Start()
        {
            _visibility = CommonTransitions.Fade(canvasGroup);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            lobby.Players.ObserveCountChanged(true).Subscribe(HandleCountChanged);
            lobby.Players.ObserveAdd().Subscribe(addData => AddClientView(addData.Value));
            lobby.Players.ObserveRemove().Subscribe(removeData => RemoveClientView(removeData.Value));
        }

        private void HandleCountChanged(int count)
        {
            title.text = $"{"Lobby".Bold().Yellow()} [{count} Connected]";
        }

        private void AddClientView(ConnectedPlayer player)
        {
            var instance = Instantiate(playerListViewPrefab, contentParent);
            instance.BindTo(player);
            _viewLookup.Add(player, instance);
        }

        private void RemoveClientView(ConnectedPlayer player)
        {
            var instance = _viewLookup[player];
            _viewLookup.Remove(player);
            instance.ClearBindings();
            Destroy(instance.gameObject);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Tab) && !_shown)
            {
                _shown = true;
                _visibility.Show();
            }
            else if (!Input.GetKey(KeyCode.Tab) && _shown)
            {
                _shown = false;
                _visibility.Hide();
            }
        }
    }
}
