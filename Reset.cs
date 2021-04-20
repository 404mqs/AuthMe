using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace CodeRewards
{

    public class Reset : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "resetpass";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "authme.resetpass" };
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (args.Length == 0)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("ResetPassUsage"), true);
                return;
            }


            if (args.Length > 1)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("ResetPassUsage"), true);
                return;
            }

            if (args.Length == 1)
            {
                var password = string.Join(" ", args[0]);

                if (!MQSPlugin.Instance.PasswordServices.IsRegistered(player.DisplayName))
                {
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("NoPassword"), true);
                    return;
                }

                if (MQSPlugin.Instance.PasswordServices.IsRegistered(player.DisplayName))
                {
                    if (MQSPlugin.Instance.PasswordServices.RightPassword(password))
                    {
                        MQSPlugin.Instance.PasswordServices.ResetPassword(player.DisplayName, player.Id, password);
                        player.Kick(MQSPlugin.Instance.Translate("ResetPass"));
                    }

                    else if (!MQSPlugin.Instance.PasswordServices.RightPassword(password))
                    {
                        UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("WrongPassword"));
                    }

                }
            }
        }
    }
}