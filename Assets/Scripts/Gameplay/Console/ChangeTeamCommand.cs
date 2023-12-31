﻿using System;
using System.Collections.Generic;
using poetools.Console;
using poetools.Console.Commands;
using Object = UnityEngine.Object;

namespace Gameplay.Console
{
    [AutoRegisterCommand]
    public class ChangeTeamCommand : ICommand
    {
        public string Name => "set-team";
        public string Help => "Requests to change your team in the multiplayer lobby.";
        public IEnumerable<string> AutoCompletions { get; } = Array.Empty<string>();

        public void Execute(string[] args, RuntimeConsole console)
        {
            Lobby lobby = Object.FindAnyObjectByType<Lobby>();
            ConnectedPlayer localPlayer = lobby.FindLocalPlayer();
            localPlayer.Rpc_ServerChangeTeam(int.Parse(args[0]));
        }

        public void Dispose()
        {
            // Do nothing.
        }
    }
}
