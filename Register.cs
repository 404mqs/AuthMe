using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;

namespace AuthMe
{

    public class Register : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "register";
        public string Help => "";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "authme.register" };
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
                UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("RegisterUsage"), true);
                return;
            }

            if (args.Length == 1)
            {
                if (!MQSPlugin.Instance.PasswordServices.IsRegistered(player.DisplayName))
                {
                    MQSPlugin.Instance.PasswordServices.RegisterPassword(player.DisplayName, player.Id, args[0]);
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("SuccesfullyRegistered"), true);
                }
                
                else
                {
                    UnturnedChat.Say(caller, MQSPlugin.Instance.Translate("NameAlreadyRegistered"), true);
                }
            }
        }
    }
}
