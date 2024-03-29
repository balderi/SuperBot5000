﻿using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using System.Net.Http;

namespace SuperBot5000.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Get response time in ms")]
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
        [Summary("Sets a timer")]
        public async Task TaskAsync(string arg)
        {
            var num = Regex.Match(arg, "\\d+").Value;
            var type = Regex.Match(arg, "[hms]").Value;

            string respType = "";

            double mult = 0;

            switch (type)
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

            System.Timers.Timer timer = new(interval);
            timer.Elapsed += Timer_Tick;

            await ReplyAsync($"Alright, I've set a timer for {num} {respType}!");
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Context.Channel.SendMessageAsync($"{Context.User.Mention} your time's up!");
            (sender as System.Timers.Timer).Stop();
        }

        [Command("status")]
        [Summary("Get http status")]
        public async Task StatusAsync(string address)
        {
            if (!address.Contains("http"))
                address = "http://" + address;
            HttpClient http = new();
            HttpRequestMessage message = new(new HttpMethod("get"), address);
            var response = await http.SendAsync(message);
            try
            {
                await ReplyAsync($"Response: `{(int)response.StatusCode} - {response.ReasonPhrase}`");
            }
            catch (WebException we)
            {
                if (we.Response != null)
                    await ReplyAsync($"Response: `{(int)((HttpWebResponse)we.Response).StatusCode} - {((HttpWebResponse)we.Response).StatusDescription}`");
                else
                    await ReplyAsync($"No response... `{we.Message}`");
            }
            catch (Exception e)
            {
                await ReplyAsync($"Something went wrong: `{e.Message}`");
            }
        }
    }
}
