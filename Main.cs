using System;
using System.IO;
using System.Text;
using InfinityScript;
using System.Collections.Generic;


namespace TeknoHook
{

    public class Main : BaseScript
    {
        private string file = "scripts\\TeknoWebHook.txt";
        private List<Entity> Entitys = new List<Entity>();

        public Main()
        {
            this.PlayerConnected += new Action<Entity>(this.playerConnected);
            if (!File.Exists(file))
            {
                File.Create(file).Close();
                using (StreamWriter sw = new StreamWriter(file, true, Encoding.UTF8))
                {
                    sw.WriteLine("WebHookURL=");
                    sw.WriteLine("BotUsername=TeknoBot Logs");
                    sw.WriteLine("BotAvatar=");
                    sw.WriteLine("SendPJoined=false");
                }
            }
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
        public static void sendWebhook(Entity player,string WebHookURL, string BotUsername, string BotAvatar, string msg)
        {
            if (WebHookURL == "")
            {
                Utilities.RawSayTo(player, "- Webhook has not been defined");
            }
            else
            {
                Http.Post(WebHookURL, new System.Collections.Specialized.NameValueCollection()
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
        }

        private void playerConnected(Entity player)
        {
            string WebHookURL = null;
            string BotUsername = null;
            string BotAvatar = null;
            string SendMsgJoined = null;

            foreach (string str in File.ReadAllLines("scripts\\TeknoWebHook.txt"))
            {
                WebHookURL = str.StartsWith("WebHookURL") ? str.Split(new char[1] { '=' })[1] : WebHookURL;
                BotUsername = str.StartsWith("BotUsername") ? str.Split(new char[1] { '=' })[1] : BotUsername;
                BotAvatar = str.StartsWith("BotAvatar") ? str.Split(new char[1] { '=' })[1] : BotAvatar;
                SendMsgJoined = str.StartsWith("SendPJoined") ? str.Split(new char[1] { '=' })[1] : SendMsgJoined;
            }

            if (SendMsgJoined == "true")
            {
                sendWebhook(
                    player,
                    WebHookURL,
                    BotUsername,
                    BotAvatar,
                    "```md\n" +
                    "-\n" +
                    $"                - {player.Name} joined the server\n" +
                    "------------------------------------------------------------------------------\n" +
                    $"-                         PlayerName: {player.Name}\n" +
                    $"# Guid: {player.GUID}                  |   IP: {player.IP}\n" +
                    $"# HWID: {player.HWID}   |   Joined at: {DateTime.Now}\n" +
                    "------------------------------------------------------------------------------\n" +
                    "                - Developed by MRX450#6329\n" +
                    "-```"
               );
            }
            Entitys.Add(player);
        }

        public override BaseScript.EventEat OnSay2(Entity player, string name, string message)
        {

            string WebHookURL = null;
            string BotUsername = null;
            string BotAvatar = null;

            foreach (string str in File.ReadAllLines("scripts\\TeknoWebHook.txt"))
            {
                WebHookURL = str.StartsWith("WebHookURL") ? str.Split(new char[1] { '=' })[1] : WebHookURL;
                BotUsername = str.StartsWith("BotUsername") ? str.Split(new char[1] { '=' })[1] : BotUsername;
                BotAvatar = str.StartsWith("BotAvatar") ? str.Split(new char[1] { '=' })[1] : BotAvatar;
            }

            sendWebhook(
                player,
                WebHookURL,
                BotUsername,
                BotAvatar,
                $"```css\n[{DateTime.Now}] {player.Name}: {message}```"
            );

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
                            player,
                            WebHookURL,
                            BotUsername,
                            BotAvatar,
                            $"```md\n# Player **{player.Name}** reported -> **{repoted.Name}**  by {reason}```"
                        );
                    }
                }
                return EventEat.EatGame;
            }
            return EventEat.EatNone;
        }
    }
}