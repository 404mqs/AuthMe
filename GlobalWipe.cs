using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace AuthMe
{
    public class WipeInfo : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Console;
        public string Name => "globalwipe";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "authme.globalwipe" };
        public void Execute(IRocketPlayer caller, string[] args)
        {
            Logger.LogWarning("AuthMe >> Attention: All the Password data has been erased from existance.");

            MQSPlugin.Instance.PasswordServices.ClearData();
            var players = Provider.clients.Select(UnturnedPlayer.FromSteamPlayer);

            foreach (var player in players.ToList())
            {
                Provider.kick(player.CSteamID, MQSPlugin.Instance.Translate("GlobalWipe"));
            } 
        }
    }
}