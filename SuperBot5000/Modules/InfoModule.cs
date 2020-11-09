using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using System.Text;

namespace SuperBot5000.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    { 
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => await ReplyAsync(echo);

        [Command("ping")]
        [Summary("Sends the response time in milliseconds")]
        public async Task PingAsync() =>
            await ReplyAsync($"Pong! ({DateTime.Now.Subtract(Context.Message.CreatedAt.LocalDateTime).TotalMilliseconds:F2} ms)");


        [Command("leg")]
        [Summary("Talk about legs...")]
        public Task LegAsync()
        {
            Context.Channel.TriggerTypingAsync();
            ReplyAsync("leg so hot");
            Thread.Sleep(1000);
            ReplyAsync("hot hot leg");
            Thread.Sleep(1000);
            ReplyAsync("leg so hot u fry an egg...");
            return Task.CompletedTask;
        }

        [Command("timer")]
        [Summary("Sets a timer for the desired time interval")]
        public async Task TaskAsync(string arg)
        {
            var num = Regex.Match(arg, "\\d+").Value;
            var type = Regex.Match(arg, "[hms]").Value;

            string respType = "";

            double mult = 0;

            switch(type)
            {
                default:
                case "s":
                    mult = 1000;
                    respType = "second(s)";
                    break;
                case "m":
                    mult = 60000;
                    respType = "minute(s)";
                    break;
                case "h":
                    mult = 3600000;
                    respType = "hour(s)";
                    break;
            }

            double interval = Convert.ToDouble(num) * mult;

            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += Timer_Tick;

            await ReplyAsync($"Alright, I've set a timer for {num} {respType}!");
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Context.Channel.SendMessageAsync($"{Context.User.Mention} your time's up!");
            (sender as System.Timers.Timer).Stop();
        }
    }
}
