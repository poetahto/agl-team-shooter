using System.Collections.Generic;
using Core;
using TMPro;
using UniRx;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreboardMenuLogic : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private ScoreboardNameView nameViewPrefab;

        [SerializeField]
        private Transform contentParent;

        [SerializeField]
        private TMP_Text title;

        private ILobby _lobby;
        private IVisibilityTransition _visibility;
        private bool _shown;
        private Dictionary<Client, ScoreboardNameView> _viewLookup = new Dictionary<Client, ScoreboardNameView>();

        private void Start()
        {
            _visibility = CommonTransitions.Fade(canvasGroup);

            if (Services.GameplaySystem.TryGetServices(out IGameplayServices services))
            {
                _lobby = services.Lobby;
                _lobby.Clients.ObserveCountChanged(true).Subscribe(HandleCountChanged);
                _lobby.Clients.ObserveAdd().Subscribe(addData => AddClientView(addData.Value));
                _lobby.Clients.ObserveRemove().Subscribe(removeData => RemoveClientView(removeData.Value));

                // foreach (var client in _lobby.Clients)
                //     AddClientView(client);
            }
        }

        private void HandleCountChanged(int count)
        {
            title.text = $"{"Lobby".Bold().Yellow()} [{count} Connected]";
        }

        private void AddClientView(Client client)
        {
            var instance = Instantiate(nameViewPrefab, contentParent);
            instance.nameText.text = $"{client.Username} [ID={client.Id}]";
            _viewLookup.Add(client, instance);
        }

        private void RemoveClientView(Client client)
        {
            var instance = _viewLookup[client];
            _viewLookup.Remove(client);
            Destroy(instance);
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
