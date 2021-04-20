using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

namespace AuthMe
{
    public class MQSPlugin : RocketPlugin<Config>
    {
        public static MQSPlugin Instance;

        public PasswordDatabase PasswordDatabase { get; private set; }

        public List<UnturnedPlayer> PlayersLogged = new List<UnturnedPlayer>();


        public PasswordServices PasswordServices { get; private set; }
        protected override void Load()
        {
            Instance = this;

            PasswordDatabase = new PasswordDatabase();
            PasswordDatabase.Reload();

            PasswordServices = gameObject.AddComponent<PasswordServices>();

            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been loaded! ");
            Logger.LogWarning("Dev: MQS#7816");
            Logger.LogWarning("Join this Discord for Support: https://discord.gg/Ssbpd9cvgp");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");

            DamageTool.damagePlayerRequested += OnPlayerDamage;

            U.Events.OnPlayerConnected += OnPlayerConnected;

            UnturnedPlayerEvents.OnPlayerChatted += OnPlayerChatted;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += OnInventoryAdd;
        }

        private void OnPlayerDamage(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            UnturnedPlayer player = UnturnedPlayer.FromCSteamID(parameters.killer);

            var user = MQSPlugin.Instance.PasswordServices.database.Data.FirstOrDefault(x => x.Name.Equals(player.DisplayName));
            LoggedComponent L = player.GetComponent<LoggedComponent>();
            if (Instance.Configuration.Instance.RestrictDamageOnLogging)
            {
                if (player == null)
                {
                    if (L.Logged == false)
                    {
                        shouldAllow = false;
                    }
                }

                else if (player != null)
                {
                    if (L.Logged == false)
                    {
                        shouldAllow = false;
                    }
                }
            }
        }

        private void OnInventoryAdd(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P)
        {
            if (MQSPlugin.Instance.Configuration.Instance.RestrictLootOnLogging)
            {
                LoggedComponent L = player.GetComponent<LoggedComponent>();
                if (L.Logged == false)
                {
                    player.Inventory.removeItem((byte)inventoryGroup, inventoryIndex);
                    UnturnedChat.Say(player, MQSPlugin.Instance.Translate("MustBeLoggedToLoot"), true);
                }
            }
        }
        private void OnPlayerChatted(UnturnedPlayer player, ref UnityEngine.Color color, string message, EChatMode chatMode, ref bool cancel)
        {
            var user = MQSPlugin.Instance.PasswordServices.database.Data.FirstOrDefault(x => x.Name.Equals(player.DisplayName, StringComparison.OrdinalIgnoreCase));
            LoggedComponent L = player.GetComponent<LoggedComponent>();
            if (L.Logged == false)
            {
                if (message.StartsWith("/login".ToLower()))
                {
                    return;
                }

                if (message.StartsWith("/register".ToLower()))
                {
                    return;
                }

                cancel = Instance.Configuration.Instance.RestrictChatOnLogging;
                UnturnedChat.Say(player, MQSPlugin.Instance.Translate("MustBeLoggedToChat"), true);
            }

            else if (player.DisplayName != user.Name)
            {
                cancel = Instance.Configuration.Instance.RestrictChatOnLogging;
                UnturnedChat.Say(player, MQSPlugin.Instance.Translate("MustBeRegisteredToChat"), true);
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            var user = MQSPlugin.Instance.PasswordServices.database.Data.FirstOrDefault(x => x.Name.Equals(player.DisplayName, StringComparison.OrdinalIgnoreCase));

            LoggedComponent L = player.GetComponent<LoggedComponent>();
            if (user == null)
            {
                if (L.Logged == false)
                {
                    player.Player.voice.allowVoiceChat = Instance.Configuration.Instance.AllowVoiceChatOnLogging;

                    player.Player.movement.sendPluginSpeedMultiplier(Instance.Configuration.Instance.SpeedOnLogging);
                    player.Player.movement.sendPluginJumpMultiplier(Instance.Configuration.Instance.JumpOnLogging);
                    player.Player.movement.sendPluginGravityMultiplier(Instance.Configuration.Instance.GravityOnLogging);

                    if (Instance.Configuration.Instance.GodModeOnLogging)
                    {
                        if (player.GodMode == false)
                        {
                            player.GodMode = !player.GodMode;
                        }
                    }

                    UnturnedChat.Say(player, MQSPlugin.Instance.Translate("RegisterMessage"), true);
                }
            }

            else if (user != null)
            {
                if (L.Logged == false)
                {
                    player.Player.movement.sendPluginSpeedMultiplier(0);
                    player.Player.movement.sendPluginJumpMultiplier(0);
                    player.Player.movement.sendPluginGravityMultiplier(0);

                    if (player.GodMode == false)
                    {
                        player.GodMode = !player.GodMode;
                    }

                    UnturnedChat.Say(player, MQSPlugin.Instance.Translate("LoginMessage"), true);
                }
            }
        }

        public void LogPlayer(UnturnedPlayer player)
        {
            LoggedComponent L = player.GetComponent<LoggedComponent>();
            L.Logged = true;

            player.Player.movement.sendPluginSpeedMultiplier(1);
            player.Player.movement.sendPluginJumpMultiplier(1);
            player.Player.movement.sendPluginGravityMultiplier(1);

            if (player.GodMode == true)
            {
                player.GodMode = !player.GodMode;
            }

            player.Player.voice.allowVoiceChat = true;

        }

        protected override void Unload()
        {
            Instance = null;

            Destroy(PasswordServices);
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been unloaded! ");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");

            PlayersLogged = null;
            DamageTool.damagePlayerRequested -= OnPlayerDamage;

            U.Events.OnPlayerConnected -= OnPlayerConnected;

            UnturnedPlayerEvents.OnPlayerChatted -= OnPlayerChatted;
            UnturnedPlayerEvents.OnPlayerInventoryAdded -= OnInventoryAdd;
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "RegisterMessage", "Register with /register [Password]" },
                { "LoginMessage", "Login with /login [Password]" },
                { "SuccesfullyLogged", "Succesfully Logged!" },
                { "SuccesfullyRegistered", "You have been registered! Use /login [Password] to log in."},
                { "WrongPassword", "The provided password is wrong." },
                { "RegisterUsage", "Usage: /register [Password]"},
                { "NameAlreadyRegistered", "This name is already registered!" },
                { "PasswordNotProvided", "You must provide a password!" },
                { "MustBeLoggedToChat", "You must be logged to use the Chat! /Login [Password]!" },
                { "MustBeLoggedToLoot", "You must be logged to loot items! /Login [Password]" },
                { "MustBeRegisteredToChat", "You must be registered to send messages! /Register [Password]!" },
                { "NotRegistered", "You are not registred. Use /register [Password]" },
                { "GlobalWipe", "Global Kicking required to reset passwords" },
                { "NotRegistered", "You are not registred. Use /register [Password]" },
                { "ResetPassUsage", "Usage: /resetpass [Old Password]" },
                { "ResetPass", "Your password has been deleted from our database. Rejoin to register" },
                { "NoPassword", "You do not have a password!" },
                { "WipePassUsage", "Usage: /wipepass [Player]" },
                { "PlayerNoPassword", "This player does not have a password!" },
                { "InvalidName", "Invalid name!" }


            };
    }
}