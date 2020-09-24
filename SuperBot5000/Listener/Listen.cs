using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        public static bool ForKeyWord(SocketUserMessage message)
        {
            if (DateTime.Now.Subtract(StaticResources.LastListen) < TimeSpan.FromSeconds(10))
                return false;

            string[] lol = new string[] { "ROFL", "ROFLMAO", "OLOLOLOLOL!!!!11!!one!!!!eleventyone", "LOL", "BWAHAHA" };
            Random rnd = new Random();
            Console.WriteLine("Listening for keywords");
            string regex;
            bool res;

            regex = @"(^|\. )why";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{message.Author.Mention} because you touch yourself at night...");
                return res;
            }

            regex = @"(^|\. )[^ ]+('s| was| is| should| has| had| took)";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, regex, "$1" + "Your mom" + "$2")}");
                return res;
            }

            regex = @".*(lol|hah|haha|hahaha|lul|lel|lulz).*";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{lol[rnd.Next(0, lol.Length)]}");
                return res;
            }

            regex = @"^.*?(I'm) (.*?)([!?.]|$)";
            res = Regex.IsMatch(message.Content, regex);
            if (res)
            {
                var match = Regex.Match(message.Content, regex);
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"Hi, {match.Groups[2]}, I'm {StaticResources.BotName}!");
                return res;
            }
            Console.WriteLine("No keywords found");
            return res;
        }
    }
}
