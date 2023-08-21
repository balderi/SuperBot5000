using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SuperBot5000.Listener
{
    public class ListenerResponse
    {
        private readonly SocketUserMessage _message;
        private readonly string _regex;
        private readonly string _reply;
        private readonly bool _doReplace;

        public ListenerResponse(SocketUserMessage message, string regex, string reply, bool doReplace)
        {
            _message = message;
            _regex = regex;
            _reply = reply;
            _doReplace = doReplace;
        }

        public bool IsMatch()
        {
            return Regex.IsMatch(_message.Content.ToLower(), _regex);
        }

        public string GetReply()
        {
            if (!IsMatch())
            {
                throw new InvalidOperationException("Regex is not matching");
            }

            StaticResources.LastListen = DateTime.UtcNow;

            if (_doReplace)
            {
                return $"{_message.Author.Mention} {Regex.Replace(_message.Content, _regex, _reply)}";
            }
            else
            {
                return _reply.Replace("#mention", _message.Author.Mention);
            }
        }
    }
}
