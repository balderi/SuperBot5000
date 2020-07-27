﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SuperBot5000
{
    class Program
    {
        CommandService _commandService;
        CommandHandler _commandHandler;

        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        IServiceProvider _services;

        System.Timers.Timer _timer;

        public async Task MainAsync()
        {
            StaticResources.Init();

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");

            await Log(new LogMessage(LogSeverity.Info, "Main", $"Currently contains {Users.UserList.GetUserList().GetNumberOfUsers()} users."));

            Environment.SetEnvironmentVariable("DiscordToken", File.ReadAllText("../../token.txt"));
            _client = new DiscordSocketClient();

            _client.Log += Log;

            _commandService = new CommandService();

            var init = new Initialize(_commandService, _client);

            _services = init.BuildServiceProvider();

            _commandHandler = new CommandHandler(_services, _client, _commandService);
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot,
                Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += Timer_Tick;

            _timer.Start();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
