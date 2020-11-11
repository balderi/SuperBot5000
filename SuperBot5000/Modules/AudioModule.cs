using Discord;
using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.Events;
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
        private readonly AudioService _oldService;
        private readonly IAudioService _service;

        public AudioModule(AudioService service, IAudioService node)
        {
            _oldService = service;
            _service = node;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinAsync()
        {
            await _oldService.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveAsync()
        {
            await _oldService.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song)
        {
            if (_service.GetPlayer<LavalinkPlayer>(Context.Guild.Id) != null)
            {
                await ReplyAsync($"Already playing `{_service.GetPlayer<LavalinkPlayer>(Context.Guild.Id).CurrentTrack.Title}`... queue is coming soon™...");
                return;
            }

            var player = _service.GetPlayer<LavalinkPlayer>(Context.Guild.Id) 
                ?? await _service.JoinAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel.Id);
            var myTrack = await _service.GetTrackAsync(song, Lavalink4NET.Rest.SearchMode.YouTube);
            await ReplyAsync($"Now playing `{myTrack.Title}`");
            await player.PlayAsync(myTrack);
        }
    }
}
