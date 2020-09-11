using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        public static bool ForKeyWord(SocketUserMessage message)
        {
            Console.WriteLine("Listening for keywords");
            var regex = @"(^|\. )[^ ]+ ('s| was| is| should| has| had| took)";
            var res = Regex.IsMatch(message.Content, regex);
            if (res)
            {
                Console.WriteLine("Keywords found");
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, regex, "$1" + "Your mom" + "$2")}");
            }
            return res;
        }
    }
}
