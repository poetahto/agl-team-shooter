using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Gameplay.UI
{
    public class ConnectedPlayerListUIView : MonoUIView<ConnectedPlayer>
    {
         [SerializeField] private TMP_Text displayText;

        private IDisposable _bindings;

        public override void BindTo(ConnectedPlayer player)
        {
            displayText.enabled = true;

            IDisposable loadoutChange = player.ObserveLoadoutChanged().Subscribe(_ => UpdateText(player));
            IDisposable nameChange = player.ObserveNameChanged().Subscribe(_ => UpdateText(player));
            IDisposable teamChange = player.ObserveTeamChanged().Subscribe(_ => UpdateText(player));
            IDisposable playerStateChange = player.ObservePlayerStateChanged().Subscribe(_ => UpdateText(player));
            UpdateText(player);

            _bindings = StableCompositeDisposable.Create(loadoutChange, nameChange, teamChange, playerStateChange);
        }

        private void UpdateText(ConnectedPlayer player)
        {
            displayText.text = $"{player.syncedPlayerName} {player.syncedLoadout}, Team {player.syncedTeamId} [{player.syncedPlayerState}]";
        }

        public override void ClearBindings()
        {
            displayText.enabled = false;
            _bindings?.Dispose();
        }
    }
}
