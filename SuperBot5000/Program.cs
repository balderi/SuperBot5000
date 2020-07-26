﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Globalization;
using System.IO;
using Discord.Rest;

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

            _client.Connected += Connected;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Connected()
        {
            if (File.Exists("pullmyfile"))
            {
                Log(new LogMessage(LogSeverity.Debug, "FileIO", "File exists"));
                string m;
                ulong id = Convert.ToUInt64(File.ReadAllText("pullmyfile"));
                Log(new LogMessage(LogSeverity.Debug, "FileIO", $"ID: {id}"));
                try
                {
                    File.Delete("pullmyfile");
                    m = "I have pulled!";
                    Log(new LogMessage(LogSeverity.Debug, "FileIO", "pullmyfile deleted"));
                }
                catch (Exception e)
                {
                    File.Create("stupidfile");
                    m = e.Message;
                    Log(new LogMessage(LogSeverity.Debug, "FileIO", $"stupidfile created: {e.Message}"));
                }

                var c = _client.GetChannel(id) as IMessageChannel;
                c.SendMessageAsync(m);
            }
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
