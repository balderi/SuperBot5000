using Discord.Commands;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        [Command("meme")]
        [Summary("Prints a super fresh meme")]
        public Task MemeAsync() =>
            Context.Channel.SendFileAsync(StaticResources.GetRandomMemePath(), "The freshest memes! 👌");
    }
}
