using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;

namespace DefaultNamespace
{
    public class PayloadGame : GameplayBehavior
    {
        public List<PlayerData> Spectators;
        public List<PlayerData> Team1;
        public List<PlayerData> Team2;

        public async UniTask Run(GameSetupState state)
        {
            await Services.GameplayRunner.SceneLoader.Server_LoadScene(state.SelectedMap);
            Spectators = state.Spectators;
            Team1 = state.Team1;
            Team2 = state.Team2;
        }
    }
}
