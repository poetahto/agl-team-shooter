using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Gameplay
{
    public class TeamDefinition : MonoBehaviour
    {
        private static Dictionary<int, TeamDefinition> _teamLookup;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _teamLookup = new Dictionary<int, TeamDefinition>();
        }

        public int id;
        public string displayName = "Team";
        public Color displayColor = Color.white;

        public string ColoredName => displayName.Color(displayColor);

        private void Awake()
        {
            _teamLookup.Add(id, this);
        }

        private void OnDestroy()
        {
            _teamLookup.Remove(id);
        }

        public static TeamDefinition FindTeam(int id)
        {
            return _teamLookup[id];
        }

        public static IEnumerable<TeamDefinition> GetTeams()
        {
            return _teamLookup.Values;
        }

        public static int GetTeamCount()
        {
            return _teamLookup.Count;
        }
    }

    public static class TeamDefinitionExtensions
    {
        public static TeamDefinition GetTeam(this ConnectedPlayer player)
        {
            return TeamDefinition.FindTeam(player.syncedTeamId);
        }
    }
}
