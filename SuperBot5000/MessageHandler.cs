using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000
{
    public class MessageHandler
    {
        Random rnd;
        List<string> memes;

        private static MessageHandler _instance = null;

        private MessageHandler()
        {
            rnd = new Random(DateTime.Now.Millisecond);
            memes = new List<string>();

            var wat = Directory.GetFiles("memes");

            foreach (var m in wat)
            {
                memes.Add(m);
            }
        }

        public static MessageHandler GetHandler()
        {
            if (_instance == null)
                _instance = new MessageHandler();
            return _instance;
        }

        public async Task HandleMessage(SocketMessage message)
        {
            switch(message.Content.ToLower())
            {
                case "!ping":
                    await message.Channel.SendMessageAsync($"Pong! ({DateTime.Now.Subtract(message.CreatedAt.LocalDateTime).TotalMilliseconds:F2} ms)");
                    break;
                case "!meme":
                    await message.Channel.SendFileAsync(memes[rnd.Next(0, memes.Count)], text: "The freshest memes!");
                    break;
                default:
                    break;
            }
        }
    }
}
