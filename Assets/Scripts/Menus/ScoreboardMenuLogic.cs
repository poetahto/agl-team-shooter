using System;
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

        private Lobby _lobby;
        private IVisibilityTransition _visibility;
        private bool _shown;
        private Dictionary<ClientData, ScoreboardNameView> _viewLookup = new Dictionary<ClientData, ScoreboardNameView>();

        private void Start()
        {
            _visibility = CommonTransitions.Fade(canvasGroup);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            print("init scoreboard");
            _lobby = Services.GameplayRunner.Systems.lobby;
            _lobby.Clients.ObserveCountChanged(true).Subscribe(HandleCountChanged);
            _lobby.Clients.ObserveAdd().Subscribe(addData => AddClientView(addData.Value));
            _lobby.Clients.ObserveRemove().Subscribe(removeData => RemoveClientView(removeData.Value));

            foreach (var client in _lobby.Clients)
                AddClientView(client);

            HandleCountChanged(_lobby.Clients.Count);
        }

        private void HandleCountChanged(int count)
        {
            title.text = $"{"Lobby".Bold().Yellow()} [{count} Connected]";
        }

        private void AddClientView(ClientData client)
        {
            var instance = Instantiate(nameViewPrefab, contentParent);
            instance.nameText.text = $"{client.Username}";
            _viewLookup.Add(client, instance);
        }

        private void RemoveClientView(ClientData client)
        {
            ScoreboardNameView instance = _viewLookup[client];
            _viewLookup.Remove(client);
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
