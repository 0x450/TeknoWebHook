using System;
using InfinityScript;


namespace TeknoHook
{

	public class TeknoWebHook : BaseScript
	{
        public TeknoWebHook()
        {
            this.PlayerConnected += new Action<Entity>(this.playerConnected);
        }

        public static void sendWebhook(string WebHook, string BotUsername, string BotAvatar, string msg)
        {
            Http.Post(WebHook, new System.Collections.Specialized.NameValueCollection()
            {
                {
                    "username",
                    BotUsername
                },
                {
                    "avatar_url",
                    BotAvatar
                },
                {
                    "content",
                    msg
                }
            });
        }

        private void playerConnected(Entity player)
        {
            sendWebhook(
                "",//WebhookURL
                "TeknoBot Logs",
                "",//Webhook Avatar IMG
                $"PlayerName: {player.Name}   |   Guid: {player.GUID}\nHWID: {player.HWID}   |   IP: {player.IP}\n   Developed by MRX450#6329"
            );
        }
    }
}