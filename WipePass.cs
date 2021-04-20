using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;

namespace CodeRewards
{
    public class WipePass : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "wipepass";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "authme.wipepass" };
        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length == 0)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("WipePassUsage"), true);
                return;
            }


            if (args.Length > 1)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("WipePassUsage"), true);
                return;
            }
            UnturnedPlayer player = UnturnedPlayer.FromName(args[0]);

            if (args.Length == 1 && player == null)
            {
                UnturnedChat.Say(MQSPlugin.Instance.Translate("InvalidName"), true);
                return;
            }

            else if (args.Length == 1)
            {
                var name = player.DisplayName;

                if (!MQSPlugin.Instance.PasswordServices.IsRegistered(name))
                {
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("PlayerNoPassword"), true);
                }

                else if (player != null && name != null)
                {
                    if (MQSPlugin.Instance.PasswordServices.IsRegistered(name))
                    {
                        MQSPlugin.Instance.PasswordServices.AdminResetPassword(name);
                        player.Kick(MQSPlugin.Instance.Translate("ResetPass"));
                    }
                }
            }



        }
    }
}