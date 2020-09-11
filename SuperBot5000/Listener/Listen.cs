using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        public static bool ForKeyWord(SocketUserMessage message)
        {
            var lol = new string[]{ "ROFL", "ROFLMAO", "OLOLOLOLOL!!!!11!!one!!!!eleventyone", "LOL", "BWAHAHA" };
            var rnd = new Random();
            Console.WriteLine("Listening for keywords");
            var regex = @"(^|\. )[^ ]+('s| was| is| should| has| had| took)";
            var res = Regex.IsMatch(message.Content, regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, regex, "$1" + "Your mom" + "$2")}");
                return res;
            }

            regex = @".*(lol|ha|hah|haha|hahaha|lul|lel|lulz).*";
            res = Regex.IsMatch(message.Content.ToLower(), regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{lol[rnd.Next(0, lol.Length)]}");
                return res;
            }

            return res;
        }
    }
}
