using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SuperBot5000.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace SuperBot5000
{
    public class StaticResources
    {
        static Random rnd ;
        static List<string> memes;

        static string[] slotsEmoji;

        public static void Init()
        {
            rnd = new Random(DateTime.Now.Millisecond);
            memes = Directory.GetFiles("memes").ToList();
            slotsEmoji = new string[]
            {
                "🍇", "🍌", "🍒", "❤️", "🔔", "🏆", "🎺", "💎", "💰", "👑", "⚜️", "⭐", "🎲", "🍀", "🍋", "7️⃣", "🙂"
            };
        }

        public static string GetRandomMemePath() => memes[rnd.Next(0, memes.Count)];

        public static int GetRandomSlotsValue() => rnd.Next(0, slotsEmoji.Length);

        public static int GetTotalSlots() => slotsEmoji.Length - 1;

        public static string GetSlotsEmoji(int id) => id >= 0 && id < slotsEmoji.Length ? slotsEmoji[id] : "Error!";

        public static bool ValidateAdminUser(SocketCommandContext context)
        {
            var du = context.User as SocketGuildUser;
            var role = (du as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Admin");
            return du.Roles.Contains(role);
        }

        public static List<IRole> GetRole(SocketCommandContext context, string[] roleNames)
        {
            return context.Guild.Roles.Where(x => x.Name == "Bot") as List<IRole>;
        }

        public static string GitFormat(string message)
        {
            var temp = message.Split('\n');
            try
            {
                return $"The commit `{temp[0]}` was submitted by `{temp[1]}` on `{temp[2]}` with the message `{temp[3]}`";
            }
            catch(Exception e)
            {
                return $"An error occurred: {e.Message}\nRaw message recieved: {message}";
            }
        }
    }
}
