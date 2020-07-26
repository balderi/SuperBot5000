using Discord;
using Discord.Commands;
using SuperBot5000.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class ShopModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("shop")]
        [Summary("Show the shop")]
        public async Task ShopAsync()
        {
            var sb = new StringBuilder("Shop is coming soon™... but look at all the stuff you can buy!\n");
            var emoji = Context.Guild.Emotes;
            foreach (var e in emoji)
            {
                sb.AppendLine($"<:{e.Name}:{e.Id}> ({e.Name}) - Price: TBD");
            }
            await ReplyAsync(sb.ToString());
        }
    }
}
