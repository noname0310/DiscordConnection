using System;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("DiscordConnection", "noname", "1.0.1")]
    [Description("Send ConnectionMsg To Discord")]
    class DiscordConnection : RustPlugin
    {
        [PluginReference]
        Plugin PlayerList, DiscordMessages;

        private new void LoadDefaultConfig()
        {
            PrintWarning("Creating a new configuration file");
            Config.Clear();

            Config["ConnectMessage"] = "{0}님이 서버에 입장하셨습니다";
            Config["DisconnectMessage"] = "{0}님이 서버에서 퇴장하셨습니다(사유:{1})";
            Config["PlayerListMessage"] = "현재 {0}명이 접속중입니다\n{1}";
            Config["PlayerListIsNullMessage"] = "현재 서버에 아무도 없습니다";
            Config["SendPlayerList"] = true;
            Config["WebhookUrl"] = "https://support.discordapp.com/hc/en-us/articles/228383668-Intro-to-Webhooks";
            SaveConfig();
        }

        private void OnPlayerInit(BasePlayer player)
        {
            if (Convert.ToBoolean(Config["SendPlayerList"]) == true)
            {
                string FullMsg = "```" + string.Format(Config["ConnectMessage"].ToString(), player.displayName) + "\n\n" + (string)PlayerList.Call("PlayersListAPI") + "```";
                DiscordMessages?.Call("API_SendTextMessage", Config["WebhookUrl"].ToString(), FullMsg, false);
            }
            else
            {
                DiscordMessages?.Call("API_SendTextMessage",
                Config["WebhookUrl"].ToString(),
                    string.Format(Config["ConnectMessage"].ToString(),
                    player.displayName),
                false);
            }
        }

        private void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (Convert.ToBoolean(Config["SendPlayerList"]) == true)
            {
                string FullMsg = "```" + string.Format(Config["DisconnectMessage"].ToString(), player.displayName, reason) + "\n\n" + (string)PlayerList.Call("PlayersListAPI") + "```";
                DiscordMessages?.Call("API_SendTextMessage", Convert.ToString(Config["WebhookUrl"]), FullMsg, false);
            }
            else
            {
                DiscordMessages?.Call("API_SendTextMessage",
                    Config["WebhookUrl"].ToString(),
                    string.Format(Config["DisconnectMessage"].ToString(),
                    player.displayName, reason),
                false);
            }
        }
    }
}