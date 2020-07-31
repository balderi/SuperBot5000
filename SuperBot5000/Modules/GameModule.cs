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

            await ReplyAsync("Unknown argument - type `!slots help` to get help (it seems like you need it...)");
        }

        [Command("multislots")]
        [Summary("Play multislots - use `slots help` to get help")]
        public async Task MultislotsAsync(string arg = "3", long bet = 10)
        {
            if(arg == "help")
            {
                await ReplyAsync(embed: Slots.Help());
                return;
            }


            if (!int.TryParse(arg, out int times))
            {
                await ReplyAsync("Unknown argument - type `!multislots help` to get help (it seems like you need it...)");
                return;
            }

            if (times > 10)
            {
                await ReplyAsync("Multislots has a limit of 10 games at once.");
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
                Title = joke.Title,
                Description = joke.Body + "\n___",
            };

            retval.WithFooter("From https://github.com/taivop/joke-dataset");

            await ReplyAsync(embed: retval.Build());
        }

        [Command("timmy")]
        [Summary("See what little Timmy is up to")]
        public async Task TimmyAsync()
        {
            TimmyPoem poem = Timmy.GetPoem();
            var retval = new EmbedBuilder()
            {
                Description = $"{poem.Body}\n\n[Context]({poem.Link})",
            };

            await ReplyAsync(embed: retval.Build());
        }
    }
}
