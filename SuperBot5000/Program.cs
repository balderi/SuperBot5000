using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Globalization;
using System.IO;
using System.Linq;
using SuperBot5000.Users;
using System.Collections.Generic;
using System.Data;

namespace SuperBot5000
{
    class Program
    {
        CommandService _commandService;
        CommandHandler _commandHandler;

#pragma warning disable IDE0060 // Remove unused parameter
        public static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
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

            await Log(new LogMessage(LogSeverity.Info, "Main", $"Currently contains {UserList.GetUserList().GetNumberOfUsers()} users."));

            Environment.SetEnvironmentVariable("DiscordToken", File.ReadAllText("../../token.txt"));
            _client = new DiscordSocketClient();

            _client.Log += Log;

            _commandService = new CommandService();

            var init = new Initialize(_commandService, _client);

            _services = init.BuildServiceProvider();

            _commandHandler = new CommandHandler(_services, _client, _commandService);
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += Timer_Tick;

            _timer.Start();

            _client.Ready += On_Ready;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task On_Ready()
        {
#if DEBUG
            await _client.SetGameAsync("Maintenance", type: ActivityType.Listening);
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
#else
            await _client.SetGameAsync("!help", type: ActivityType.Listening);
#endif

            if (File.Exists("pullmyfile"))
            {
                if(ulong.TryParse(File.ReadAllText("pullmyfile"), out ulong chanID))
                {
                    var c = _client.GetChannel(chanID) as IMessageChannel;
                    await c.SendMessageAsync("I have pulled the latest commit!");
                    File.Delete("pullmyfile");
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var _guildUsers = _client.GetGuild(252801284439015424).Users;
            //update users
            foreach (SocketUser user in _guildUsers)
            {
                UserList.GetUserList().GetUser(user);
            }
            _users = UserList.GetUserList().Users;
            _names = _users.Select(x => x.Name).ToList();
            var olUsers = _guildUsers.Where(x => x.Status != UserStatus.Offline).Where(x => _names.Contains(x.Mention)).Select(x => x.Mention).ToList();
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
