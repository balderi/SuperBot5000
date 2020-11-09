using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SuperBot5000.Modules
{
    public class VoiceModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("voicetest")]
        [Summary("Test voice")]
        public async Task TestVoiceAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            IVoiceState voiceState = (IVoiceState)Context.User;

            if(voiceState.VoiceChannel == null)
            {
                await ReplyAsync($"{Context.User.Mention} is not in a voice channel.");
                return;
            }

            var channel = Context.Guild.GetVoiceChannel(voiceState.VoiceChannel.Id);
            _ = ConnectToVoice(channel);
        }

        private async Task ConnectToVoice(SocketVoiceChannel voiceChannel)
        {
            if (voiceChannel == null)
            {
                await ReplyAsync($"{Context.User.Mention} is not in a voice channel.");
                return;
            }

            Console.WriteLine($"Connecting to channel {voiceChannel.Name}");
            var conn = await voiceChannel.ConnectAsync();
            Console.WriteLine($"Connected to channel {voiceChannel.Name}");

            var info = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $@"-i ""test.wav"" -ac 2 -f s16le -ar 48000 pipe:1",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var ffmpeg = Process.Start(info);

            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = conn.CreatePCMStream(Discord.Audio.AudioApplication.Voice);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
            return;
        }

        [Command("leave")]
        [Summary("Leave voice")]
        public async Task LeaveVoiceAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            IVoiceState voiceState = (IVoiceState)Context.User;

            var channel = Context.Guild.GetVoiceChannel(voiceState.VoiceChannel.Id);

        }
    }
}
