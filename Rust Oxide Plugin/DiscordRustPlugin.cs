using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Text;
#if RUST
using Oxide.Core;
using Newtonsoft.Json;
#endif
using MorganSkilly;

namespace Oxide.Plugins
{

#if RUST
    [Info("Easy Discord Integration", "MorganSkilly", "1.0.0")]
    [Description("Allows easy communication from a Rust server to a Discord server")]
#endif

#if !RUST
    class DiscordRustPlugin
    {
        static void Main(string[] args)
        {
            EasyDiscordIntegration.Send("Easy Discord Integration Started!");
            EasyDiscordIntegration.Send("Easy Discord Integration Started!", "Username");
        }
    }
#endif

#if RUST
    public class DiscordRustPlugin : RustPlugin
    {
        private const string permUse = "easydiscord.use";
        private void Init()
        {
            cmd.AddChatCommand(config.command, this, nameof(cmdSendDiscord));
            permission.RegisterPermission(permUse, this);
        }

        public static ConfigData config;

        //adjust these to match your webhook setup in the config file

        public class ConfigData
        {
            [JsonProperty(PropertyName = "Command")]
            public string command;

            [JsonProperty(PropertyName = "Webhook")]
            public string webhook;

            [JsonProperty(PropertyName = "WebhookConnections")]
            public string webhookconnections;
        }

        private ConfigData GetDefaultConfig()
        {
            return new ConfigData
            {
                command = "discord",
                webhook = "DEFAULT WEBHOOK GOES HERE",
                webhookconnections = "CONNECTIONS WEBHOOK GOES HERE",
            };
        }
        
        private void cmdSendDiscord(BasePlayer player, string command, string[] args)
        {
            if (permission.UserHasPermission(player.UserIDString, permUse) == false)
            {
                Message(player, "Permission");
                return;
            }

            if (args == null || args?.Length < 1)
            {
                Message(player, "Usage");
                return;
            }

            EasyDiscordIntegration.Send(args[0], player.displayName);
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();

            try
            {
                config = Config.ReadObject<ConfigData>();

                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                PrintError("Configuration file is corrupt! Unloading plugin...");
                Interface.Oxide.RootPluginManager.RemovePlugin(this);
                return;
            }

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config);
        }
                
        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                {"Usage", "Usage:\n/discord message"},
                {"Permission", "You don't have permission to use that!"},
                {"Error", "Looks as you did something wrong!"}
            }, this);
        }
        
        private void Message(BasePlayer player, string messageKey, params object[] args)
        {
            if (player == null)
            {
                return;
            }

            var message = GetMessage(messageKey, player.UserIDString, args);
            player.SendConsoleCommand("chat.add", (object) 0, (object) message);
        }

        private string GetMessage(string messageKey, string playerID, params object[] args)
        {
            return string.Format(lang.GetMessage(messageKey, this, playerID), args);
        }
    }
#endif

}
