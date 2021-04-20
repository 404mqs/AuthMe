using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace AuthMe
{

    public class Login : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "login";
        public string Help => "";
        public string Syntax => throw new NotImplementedException();
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "authme.login" };
        public void Execute(IRocketPlayer caller, string[] args)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (args.Length == 0)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("PasswordNotProvided"), true);
                return;
            }

            if (args.Length > 1)
            {
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("LoginUsage"), true);
                return;
            }

            if (args.Length == 1)
            {
                var password = string.Join(" ", args[0]);

                if (MQSPlugin.Instance.PasswordServices.RightPassword(password))
                {
                    MQSPlugin.Instance.LogPlayer(player);
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("SuccesfullyLogged"), true);
                }

                else if (!MQSPlugin.Instance.PasswordServices.RightPassword(password))
                {
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("WrongPassword"), true);
                    return;
                }

                else if (!MQSPlugin.Instance.PasswordServices.IsRegistered(caller.DisplayName))
                {
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("NotRegistered"), true);
                    return;
                }
            }
        }
    }
}