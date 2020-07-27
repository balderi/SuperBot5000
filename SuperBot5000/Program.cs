﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Globalization;
using System.IO;
using System.Linq;
using SuperBot5000.Users;
using System.Collections.Generic;

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

        List<User> _users;
        List<string> _names;

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
            _users = UserList.GetUserList().Users;
            _names = _users.Select(x => x.Name).ToList();
            var olUsers = _client.GetGuild(252801284439015424).Users.Where(x => _names.Contains(x.Mention)).Select(x => x.Mention).ToList();
            foreach(User u in _users.Where(x => olUsers.Contains(x.Name)))
            {
                u.IncrementOLPoints();
                u.TryRedeemOLPoints();
            }
            UserList.GetUserList().SaveList();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
