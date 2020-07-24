using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("help")]
        [Summary("Displays help")]
        public async Task HelpAsync()
        {
            List<CommandInfo> commands = Commands.Commands.ToList();

            var retval = new EmbedBuilder()
            {
                Title = "SuperBot5000 - Help",
                Description = "A super basic discord-bot using the discord.net framework.",
                Color = Color.Blue
            };

            StringBuilder sb = new StringBuilder("All commands should be prepended with either an exclamation point (`!`) or a mention (`@botname`)\n\n");

            foreach(CommandInfo c in commands)
            {
                sb.AppendLine($" • `{c.Name}` - {c.Summary}");
            }

            retval.AddField("Commands", sb.ToString());

            await ReplyAsync(embed: retval.Build());
        }
    }
}
