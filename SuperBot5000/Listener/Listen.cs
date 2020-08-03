using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;

namespace SuperBot5000.Listener
{
    public class Listen
    {
        public static bool ForKeyWord(SocketUserMessage message)
        {
            if(Regex.IsMatch(message.Content, @"(^|\. )[^ ]+ (was|is|should|has|had|took)"))
            {
                message.Channel.SendMessageAsync($"{message.Author.Mention} {Regex.Replace(message.Content, @"(^|\. )[^ ]+ (was|is|should|has|had|took)", "$1" + "Your mom " + "$2")}");
                return true;
            }
            return false;
        }
    }
}
