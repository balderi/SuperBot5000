using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Text.RegularExpressions;

namespace SuperBot5000.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => ReplyAsync(echo);

        [Command("ping")]
        [Summary("Sends the response time in milliseconds")]
        public Task PingAsync() =>
            ReplyAsync($"Pong! ({DateTime.Now.Subtract(Context.Message.CreatedAt.LocalDateTime).TotalMilliseconds:F2} ms)");

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

            Timer timer = new Timer(interval);
            timer.Elapsed += Timer_Tick;

            await ReplyAsync($"Alright, I've set a timer for {num} {respType}!");
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Context.Channel.SendMessageAsync($"{Context.User.Mention} your time's up!");
            (sender as Timer).Stop();
        }
    }
}
