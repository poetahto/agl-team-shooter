using System.Collections.Generic;
using Core;
using FishNet.Object;
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
        private ScoreboardNameView nameViewPrefab;

        [SerializeField]
        private Transform contentParent;

        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private Lobby lobby;

        private IVisibilityTransition _visibility;
        private bool _shown;
        private Dictionary<ConnectedPlayer, ScoreboardNameView> _viewLookup = new Dictionary<ConnectedPlayer, ScoreboardNameView>();

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
            var instance = Instantiate(nameViewPrefab, contentParent);

            // todo: auto sync this w/ the state in-case it changes - use the MonoView pattern
            instance.nameText.text = $"{player.syncedPlayerName} {player.syncedLoadout}, Team {player.syncedTeamId}";
            _viewLookup.Add(player, instance);
        }

        private void RemoveClientView(ConnectedPlayer player)
        {
            ScoreboardNameView instance = _viewLookup[player];
            _viewLookup.Remove(player);
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
