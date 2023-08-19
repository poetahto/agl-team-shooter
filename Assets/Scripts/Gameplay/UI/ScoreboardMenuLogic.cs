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
        private Dictionary<PlayerData, ScoreboardNameView> _viewLookup = new Dictionary<PlayerData, ScoreboardNameView>();

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

        private void AddClientView(PlayerData player)
        {
            var instance = Instantiate(nameViewPrefab, contentParent);
            instance.nameText.text = $"{player.Username}";
            _viewLookup.Add(player, instance);
        }

        private void RemoveClientView(PlayerData player)
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
