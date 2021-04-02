using System;
using InfinityScript;
using System.Collections.Generic;


namespace TeknoHook
{

	public class Main : BaseScript
	{
        private string WebHookURL = "";//WebhookURL
        private string BotUsername = "TeknoBot Logs";//BotUsername
        private string BotAvatar = "";//BotAvatar

        public List<Entity> Entitys = new List<Entity>();
        public Main()
        {
            this.PlayerConnected += new Action<Entity>(this.playerConnected);
        }

        Entity FindByName(string name)
        {

            int cont = 0;
            Entity player = null;
            foreach (Entity Player in Entitys)
            {
                if (0 <= Player.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    player = Player;
                    cont++;
                }
            }
            if (cont > 1) { return null; }
            if (cont == 1) { return player; }

            return null;
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
                WebHookURL,
                BotUsername,
                BotAvatar,
                $"PlayerName: {player.Name}   |   Guid: {player.GUID}\nHWID: {player.HWID}   |   IP: {player.IP}\n   Developed by MRX450#6329"
            );
            Entitys.Add(player);
        }

        public override BaseScript.EventEat OnSay2(Entity player, string name, string message){
            string[] Array = message.Split(' ');

            string reason = "";

            if (Array[0].Equals("!report"))
            {
                if (Array.Length <= 2)
                {
                    Utilities.RawSayTo(player, " ^7Usage: ^1!report ^2<player> <reason>");
                    return BaseScript.EventEat.EatGame;
                }
                if (Array.Length >= 3)
                {
                    Entity repoted = FindByName(Array[1]);
                    if (repoted == null)
                    {
                        Utilities.RawSayTo(player, "The player was not found");
                    }
                    else
                    {
                        for (int i = 2; i < Array.Length; i++)
                        {
                            reason += " " + Array[i];
                        }
                        Utilities.RawSayTo(player, "Report sent successfully!");
                        sendWebhook(
                            WebHookURL,
                            BotUsername,
                            BotAvatar,
                            $"Player **{player.Name}** reported -> **{repoted.Name}** by {reason}"
                        );
                    }
                }
                return EventEat.EatGame;
            }
            return EventEat.EatNone;
        }
    }
}