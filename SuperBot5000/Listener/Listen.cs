using Discord.WebSocket;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        public static bool ForKeyWord(SocketUserMessage message)
        {
            double wait = DateTime.Now.Subtract(StaticResources.LastListen).TotalSeconds;

            if (wait < 5)
            {
                Console.WriteLine($"Not listening yet - too soon... ({10 - wait} seconds left)");
                return false;
            }

            //StaticResources.LastListen = DateTime.Now;
            string[] lol = new string[] { "ROFL", "ROFLMAO", "OLOLOLOLOL!!!!11!!one!!!!eleventyone", "LOL", "BWAHAHA" };
            string[] bangalore = File.ReadAllLines("bangalore.txt");
            Random rnd = new Random();
            Console.WriteLine("Listening for keywords");
            string regex;
            bool res;

            if(message.Content.ToLower().Contains("apex"))
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync(bangalore[rnd.Next(bangalore.Length)]);
                return true;
            }

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
                message.Channel.SendMessageAsync($"Hi, {match.Groups[2]}, I'm {StaticResources.BotName ?? "testbot"}!");
                return res;
            }
            Console.WriteLine("No keywords found");
            return res;
        }
    }
}
