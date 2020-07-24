using Discord;
using Discord.Commands;
using SuperBot5000.Games;
using System;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        [Command("slots")]
        [Summary("Play slots - use `slots help` to get help")]
        public async Task SlotsAsync(string arg = "10")
        {
            if (arg.ToLower() == "help")
            {
                await ReplyAsync(embed: Slots.Help());
                return;
            }

            if(long.TryParse(arg, out long bet))
            {
                await ReplyAsync(message: Context.User.Mention, embed: Slots.Play(Context, bet));
                return;
            }

            await ReplyAsync("Unknown argument - type '!slots help' to get help (it seems like you need it...)");
        }

        [Command("multislots")]
        [Summary("Play multislots - use `multislots <times> <bet>` to play, e.g. `multislots 10 50` to play 10 games with a bet of 50 coins each")]
        public async Task MultislotsAsync(int times = 3, long bet = 10)
        {
            if(times > 10)
            {
                await ReplyAsync("Multislots has a limt of 10 games at once.");
                return;
            }
            try
            {
                await ReplyAsync(message: Context.User.Mention, embed: Slots.PlayMulti(Context, times, bet));
            }
            catch(Exception e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("joke")]
        [Summary("Tells a funny joke")]
        public async Task JokeAsync()
        {
            RedditJoke joke = Jokes.GetJoke();
            var retval = new EmbedBuilder()
            {
                Title = joke.title,
                Description = joke.body + "\n\n___",
            };

            retval.WithFooter("From https://github.com/taivop/joke-dataset");

            await ReplyAsync(embed: retval.Build());
        }
    }
}
