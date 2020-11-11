using CliWrap;
using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace SuperBot5000.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            IAudioClient client;

            if(ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                using (var ffmpeg = CreateStream(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync(); }
                }
            }

            //if (path.Contains(".youtu"))
            //{
            //    Console.WriteLine("It's a youtube video!");
            //    string video;

            //    if(path.Contains("watch?v="))
            //    {
            //        video = path.Split("watch?v=").Last();
            //    }
            //    else
            //    {
            //        video = path.Split("/").Last();
            //    }


            //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
            //    {
            //        Console.WriteLine("Ooooh we're trying to play it now!");
            //        var youtube = new YoutubeClient();
            //        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video);

            //        // ...or highest bitrate audio-only stream
            //        var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();

            //        if (streamInfo != null)
            //        {
            //            // Get the actual stream
            //            await using var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

            //            await using var output = client.CreatePCMStream(AudioApplication.Music);
            //            Console.WriteLine("Wrapping ffmpeg!");

            //            var cmd = stream | Cli.Wrap("ffmpeg").WithArguments("-i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1") | output;
            //            Console.WriteLine($"*** Command:\n{cmd}\n*** Args:\n{cmd.Arguments}");

            //            await cmd.ExecuteAsync();
            //        }
            //    }
            //}
            //else
            //{
            //    if (!File.Exists(path))
            //    {
            //        await channel.SendMessageAsync("File does not exist.");
            //        return;
            //    }
            //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
            //    {
            //        using (var ffmpeg = CreateProcess(path))
            //        using (var stream = client.CreatePCMStream(AudioApplication.Music))
            //        {
            //            try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
            //            finally { await stream.FlushAsync(); }
            //        }
            //    }
            //}
        }

        private Process CreateStream(string query)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = $"--default-search ytsearch -o - \"{query}\" | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }

        private Process CreateProcess(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
