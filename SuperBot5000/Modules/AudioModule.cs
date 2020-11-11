using Discord;
using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.Player;
using SuperBot5000.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        private readonly AudioService _service;
        private readonly IAudioService _node;

        public AudioModule(AudioService service, IAudioService node)
        {
            _service = service;
            _node = node;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinAsync()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveAsync()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song)
        {
            var player = _node.GetPlayer<LavalinkPlayer>(Context.Guild.Id) ?? await _node.JoinAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel.Id);
            var myTrack = await _node.GetTrackAsync(song, Lavalink4NET.Rest.SearchMode.YouTube);
            await ReplyAsync($"Now playing `{myTrack.Title}`");
            await player.PlayAsync(myTrack);
            //await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }
    }
}
