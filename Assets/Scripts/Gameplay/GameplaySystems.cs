using DefaultNamespace;
using FishNet.Object;

public interface IGame
{
}

public class GameplaySystems : NetworkBehaviour
{
    public Lobby lobby;
    public ScoreboardMenuLogic scoreboard;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        Services.GameplayRunner.Systems = this;
    }
}
