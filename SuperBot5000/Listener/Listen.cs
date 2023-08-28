using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        private static List<ListenerResponse> _responses = new();

        public static bool ForKeyWord(SocketUserMessage message)
        {
            double wait = DateTime.UtcNow.Subtract(StaticResources.LastListen).TotalSeconds;

            string[] lol = new string[] { "ROFL", "ROFLMAO", "OLOLOLOLOL!!!!11!!one!!!!eleventyone", "LOL", "BWAHAHA" };
            string[] bangalore = File.ReadAllLines("bangalore.txt");
            string yourMom = "(var|er|skulle|har|havde|tog|set|ser|så|tager)";
            Random rnd = new();
            Console.WriteLine("Listening for keywords");
            string regex;
            bool res;
            int roll = rnd.Next(100);

            regex = @"(\uD83D\uDE41|\uD83D\uDE26|\u2639|\uD83D\uDE22|\uD83D\uDE2D|\uD83D\uDE3F)";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res)
            {
                Console.WriteLine("Keywords found: sadface");
                message.AddReactionAsync(new Emoji("\u2764"));
            }

            if (message.Content.ToLower().Contains("apex"))
            {
                Console.WriteLine("Keywords found: apex");
                message.Channel.SendMessageAsync(bangalore[rnd.Next(bangalore.Length)]);
                return true;
            }

            if (message.Content.ToLower() == "ja")
            {
                Console.WriteLine("Keywords found: ja");
                message.Channel.SendMessageAsync("Nej");
                return true;
            }

            if (message.Content.ToLower() == "nej")
            {
                Console.WriteLine("Keywords found: nej");
                message.Channel.SendMessageAsync("Ja");
                return true;
            }

            if (message.Content.ToLower() == "jo")
            {
                Console.WriteLine("Keywords found: jo");
                message.Channel.SendMessageAsync("Næh");
                return true;
            }

            if (message.Content.ToLower() == "næh")
            {
                Console.WriteLine("Keywords found: næh");
                message.Channel.SendMessageAsync("Jo");
                return true;
            }


            if (wait < StaticResources.ListenDelay)
            {
                Console.WriteLine($"Not listening yet - too soon... ({StaticResources.ListenDelay - wait:N2} seconds left)");
                return false;
            }


            if (message.Content.ToLower().Contains("tænk hvis"))
            {
                Console.WriteLine("Keywords found: tænk hvis");
                message.Channel.SendMessageAsync($"Tænk hvis {message.Author.Mention} kunne synge... \uD83E\uDD14");
                return true;
            }

            regex = @"(^|)hvorfor";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res && roll < 90)
            {
                Console.WriteLine($"Keywords found: hvorfor - roll={roll} (90)");
                message.Channel.SendMessageAsync($"{message.Author.Mention} fordi du piller ved dig selv om natten...");
                return res;
            }

            regex = @"(jeg|den|det|du|vi|I|dem)[ ]{1}" + yourMom;
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res && roll < 50)
            {
                Console.WriteLine($"Keywords found: din mor - roll={roll} (50)");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, "( jeg | den | det | du | vi | I | dem )", " din mor ")}");
                return res;
            }

            regex = @"(Jeg|Den|Det|Du|Vi|I|Dem)[ ]{1}" + yourMom;
            res = Regex.IsMatch(message.Content.ToLower(), regex, RegexOptions.Multiline);
            if (res && roll < 50)
            {
                Console.WriteLine($"Keywords found: Din mor - roll={roll} (50)");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, "(^|Jeg |Den |Det |Du |Vi |I |Dem )", "Din mor ")}");
                return res;
            }

            regex = @"(^|\. )why";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res && roll < 90)
            {
                Console.WriteLine($"Keywords found: why - roll={roll} (90)");
                message.Channel.SendMessageAsync($"{message.Author.Mention} because you touch yourself at night...");
                return res;
            }

            regex = @"(^|\. )[^ ]+('s| was| is| should| has| had| took)";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res && roll < 50)
            {
                Console.WriteLine($"Keywords found: Your mom - roll={roll} (50)");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, regex, "$1" + "Your mom" + "$2")}");
                return res;
            }

            regex = @".*(lol|hah|haha|hahaha|lul|lel|lulz).*";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res && roll < 80)
            {
                Console.WriteLine($"Keywords found: lol - roll={roll} (80)");
                message.Channel.SendMessageAsync($"{lol[rnd.Next(0, lol.Length)]}");
                return res;
            }

            regex = @"^.*?(I'm) (.*?)([!?.]|$)";
            res = Regex.IsMatch(message.Content, regex);
            if (res && roll < 75)
            {
                var match = Regex.Match(message.Content, regex);
                Console.WriteLine($"Keywords found: I'm... - roll={roll} (75)");
                message.Channel.SendMessageAsync($"Hi, {match.Groups[2]}, I'm {StaticResources.BotName ?? "a bot"}!");
                return res;
            }
            Console.WriteLine($"No keywords found - roll={roll}");
            return res;
        }

        private void CreateResponseList(SocketUserMessage message)
        {
            _responses.Add(new ListenerResponse(message,
                "(^|)hvorfor",
                "fordi du piller ved dig selv om natten...",
                false));
            _responses.Add(new ListenerResponse(message,
                "(^|Jeg|Den|Det|Du|Vi|I|Dem)( var| er| skulle| har| havde| tog)",
                "$1" + "Din mor" + "$2",
                true));
            _responses.Add(new ListenerResponse(message,
                "(^|jeg|den|det|du|vi|I|dem)( var| er| skulle| har| havde| tog)",
                "$1" + "din mor" + "$2",
                true));
            _responses.Add(new ListenerResponse(message,
                "(^|\\. )[^ ]+('s| was| is| should| has| had| took)",
                "$1" + "Your mom" + "$2",
                true));
            _responses.Add(new ListenerResponse(message,
                "(^|\\. )why",
                "because you touch yourself at night...",
                false));
        }
    }
}
