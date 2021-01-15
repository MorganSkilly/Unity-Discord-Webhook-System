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

    public class DiscordRustPluginHooks : RustPlugin
    {
        private void Init()
        {
            EasyDiscordIntegration.Send("Server is starting...", "Rust Server", DiscordRustPlugin.config.webhook);
        }

        private void OnServerInitialized()
        {
            EasyDiscordIntegration.Send("Server started successfully and is ready to join!", "Rust Server", DiscordRustPlugin.config.webhook);
        }

        private void OnServerSave()
        {
            EasyDiscordIntegration.Send("Server is saving...", "Rust Server", DiscordRustPlugin.config.webhook);
        }

        private void OnServerShutdown()
        {
            EasyDiscordIntegration.Send("Server is shutting down...", "Rust Server", DiscordRustPlugin.config.webhook);
        }

        private void OnPlayerConnected(BasePlayer player)
        {
            EasyDiscordIntegration.Send(player.displayName + " has connected to the server.", "Rust Server", DiscordRustPlugin.config.webhookconnections);
        }

        private void OnPlayerDisconnected(BasePlayer player)
        {
            EasyDiscordIntegration.Send(player.displayName + " has disconnected from the server.", "Rust Server", DiscordRustPlugin.config.webhookconnections);
        }
    }

#endif
}
