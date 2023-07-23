using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class GameSetupState
    {
        public string SelectedMap;
        public readonly List<PlayerData> Spectators = new List<PlayerData>();
        public readonly List<PlayerData> Team1 = new List<PlayerData>();
        public readonly List<PlayerData> Team2 = new List<PlayerData>();
    }

    public class GameSetupMenuLogic : GameplayBehavior
    {
        [SerializeField] private GameObject target;
        [SerializeField] private string[] mapOptions;
        [SerializeField] private Transform spectatorParent;
        [SerializeField] private Transform team1Parent;
        [SerializeField] private Transform team2Parent;
        [SerializeField] private TMP_Text nameViewPrefab;
        [SerializeField] private TeamSelectionMenu teamSelectionPrefab;

        private GameSetupState _state = new GameSetupState();
        private Dictionary<PlayerData, TMP_Text> _playersToViews;

        private void Awake()
        {
            _playersToViews = new Dictionary<PlayerData, TMP_Text>();

            new ExistingMenuBuilder()
                .Register("start", new Button(() => FindObjectOfType<PayloadGame>().Run(_state).Forget()))
                .Register("quit", new Button(() => Services.GameplayRunner.StopGame().Forget()))
                .Register("maps", Dropdown.FromOptions(mapOptions, index => _state.SelectedMap = mapOptions[index]))
                .Build(target)
                .AddTo(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            // Listen for players joining and leaving the game, and update the UI accordingly.
            Lobby.Players.ObserveAdd().Subscribe(addEvent => AddNewPlayer(addEvent.Value)).AddTo(this);
            Lobby.Players.ObserveRemove().Subscribe(removeEvent => RemovePlayer(removeEvent.Value)).AddTo(this);

            foreach (var player in Lobby.Players)
                AddNewPlayer(player);
        }

        private void CreateNameView(PlayerData player)
        {
            var instance = Instantiate(nameViewPrefab, spectatorParent);
            instance.text = player.Username;

            instance.OnPointerClickAsObservable()
                .Where(clickData => clickData.button == PointerEventData.InputButton.Right)
                .Subscribe(clickData =>
                {
                    var i = Instantiate(teamSelectionPrefab, instance.transform);

                    i.team1Button.onClick.AddListener(() =>
                    {
                        AddToTeam1(player);
                        Destroy(i.gameObject);
                    });
                    i.team2Button.onClick.AddListener(() =>
                    {
                        AddToTeam2(player);
                        Destroy(i.gameObject);
                    });
                    i.spectators.onClick.AddListener(() =>
                    {
                        AddToSpectators(player);
                        Destroy(i.gameObject);
                    });
                }).AddTo(instance);

            _playersToViews.Add(player, instance);
        }

        // === Player Lifetime ===

        private void AddNewPlayer(PlayerData player)
        {
            CreateNameView(player);
            AddToSpectators(player);
        }

        private void RemovePlayer(PlayerData player)
        {
            Destroy(_playersToViews[player].gameObject);
            _playersToViews.Remove(player);
            RemoveFromAllTeams(player);
        }

        // === Player Team Management ===

        private void RemoveFromAllTeams(PlayerData data)
        {
            _state.Spectators.Remove(data);
            _state.Team1.Remove(data);
            _state.Team2.Remove(data);
        }

        private void AddToSpectators(PlayerData player)
        {
            RemoveFromAllTeams(player);
            _playersToViews[player].transform.SetParent(spectatorParent);
            _state.Spectators.Add(player);
        }

        private void AddToTeam1(PlayerData player)
        {
            RemoveFromAllTeams(player);
            _playersToViews[player].transform.SetParent(team1Parent);
            _state.Team1.Add(player);
        }

        private void AddToTeam2(PlayerData player)
        {
            RemoveFromAllTeams(player);
            _playersToViews[player].transform.SetParent(team2Parent);
            _state.Team2.Add(player);
        }
    }
}
