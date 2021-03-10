using Discord.Commands;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Microsoft.Extensions.DependencyInjection;
using SuperBot5000.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SuperBot5000
{
    public class Initialize
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;

        // Ask if there are existing CommandService and DiscordSocketClient
        // instance. If there are, we retrieve them and add them to the
        // DI container; if not, we create our own.
        public Initialize(CommandService commands = null, DiscordSocketClient client = null)
        {
            _commands = commands ?? new CommandService();
            _client = client ?? new DiscordSocketClient();
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton<CommandHandler>()
            .AddSingleton<AudioService>()
            .AddSingleton<IAudioService, LavalinkNode>()
            .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
            .AddSingleton(new LavalinkNodeOptions
            {
                RestUri = "http://localhost:8080/",
                WebSocketUri = "ws://localhost:8080/",
                Password = File.ReadAllText("../lavalink.txt"),
                BufferSize = 102400
            })
            .BuildServiceProvider();


    }

    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
        {
            _services = services;
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            if (messageParam is not SocketUserMessage message) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (message.Author.IsBot)
                return;

            bool treatAsCommand = message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos);
            
            if (treatAsCommand)
            {
                // Create a WebSocket-based command context based on the message
                var context = new SocketCommandContext(_client, message);

                // Execute the command with the command context we just
                // created, along with the service provider for precondition checks.
                //var typing = context.Channel.EnterTypingState();
                // Keep in mind that result does not indicate a return value
                // rather an object stating if the command executed successfully.
                var result = await _commands.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: _services);

                // Optionally, we may inform the user if the command fails
                // to be executed; however, this may not always be desired,
                // as it may clog up the request queue should a user spam a
                // command.
                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync($"{result.ErrorReason.Replace('.',':')} \"{message}\"");

                //typing.Dispose();
                return;
            }

            if(Listener.Listen.ForKeyWord(message))
                StaticResources.LastListen = DateTime.Now;
        }
    }
}
