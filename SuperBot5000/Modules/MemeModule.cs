using Discord.Commands;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        [Command("meme")]
        [Summary("Get a super fresh meme")]
        public async Task MemeAsync() =>
            await Context.Channel.SendFileAsync(StaticResources.GetRandomMemePath(), "The freshest memes! 👌");
    }
}
