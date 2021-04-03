using Discord;
using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using SuperBot5000.Services;
using System;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        private readonly AudioService _managerService;
        private readonly IAudioService _service;
        private QueuedLavalinkPlayer _player;

        public AudioModule(AudioService managerService, IAudioService service)
        {
            Console.WriteLine("*** Created AudioModule");

            _managerService = managerService;
            _service = service;

            _managerService.Init(_service);
        }

        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins voice")]
        public async Task JoinAsync()
        {
            GetIds();
            await _service.JoinAsync(StaticResources.CurrentGuildId, StaticResources.CurrentVoiceChannelId);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves voice")]
        public async Task LeaveAsync()
        {
            _player = await GetPlayer();
            await _player?.DisconnectAsync();
        }

        [Command("playing", RunMode = RunMode.Async)]
        [Summary("Now playing")]
        public async Task NowPlayingAsync()
        {
            _player = await GetPlayer();
            if (_player.CurrentTrack == null)
                await ReplyAsync("Queue is empty :(");
            await ReplyAsync($"Now playing: `{_player.CurrentTrack.Title}`");
        }

        [Command("vol++", RunMode = RunMode.Async)]
        [Summary("Vol += 10%")]
        public async Task VolUpAsync()
        {
            _player = await GetPlayer();
            await _player.SetVolumeAsync(_player.Volume + 0.1f);
            await ReplyAsync($"Volume is at {(int)(_player.Volume * 100)}%");
        }

        [Command("vol--", RunMode = RunMode.Async)]
        [Summary("Vol -= 10%")]
        public async Task VolDnAsync()
        {
            _player = await GetPlayer();
            await _player.SetVolumeAsync(_player.Volume - 0.1f);
            await ReplyAsync($"Volume is at {(int)(_player.Volume * 100)}%");
        }

        [Command("queue", RunMode = RunMode.Async)]
        [Summary("Show playlist queue")]
        public async Task QueueAsync()
        {
            _player = await GetPlayer();

            await ReplyAsync(AudioService.PrintQueue(_player));
        }

        [Command("play", RunMode = RunMode.Async)]
        [Summary("Play music off YouTube (direct link or search query)")]
        public async Task PlayAsync([Remainder] string song)
        {
            _player = await GetPlayer();

            var track = await _service.GetTrackAsync(song, SearchMode.YouTube);

            await _player.PlayAsync(track, true);

            await ReplyAsync($"Added `{track.Title}` to queue!\n\n{AudioService.PrintQueue(_player)}");
        }

        [Command("nadås", RunMode = RunMode.Async)]
        [Summary("wat")]
        public async Task Nadås(string full = null)
        {
            _player = await GetPlayer();

            var track = await _service.GetTrackAsync("nadås.mp3");

            if (full != null)
            {
                track = await _service.GetTrackAsync("nadåsf.wav");
            }

            await _player.PlayAsync(track, true);
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Summary("Stop music")]
        public async Task StopAsync()
        {
            if (!_service.HasPlayer(StaticResources.CurrentGuildId))
                return;
            _player = await GetPlayer();
            await _player.StopAsync();
        }

        [Command("pause", RunMode = RunMode.Async)]
        [Summary("Pause music")]
        public async Task PauseAsync()
        {
            if (!_service.HasPlayer(StaticResources.CurrentGuildId))
                return;
            _player = await GetPlayer();
            await _player.PauseAsync();
        }

        [Command("resume", RunMode = RunMode.Async)]
        [Summary("Resume music")]
        public async Task ResumeAsync()
        {
            if (!_service.HasPlayer(StaticResources.CurrentGuildId))
                return;
            _player = await GetPlayer();
            await _player.ResumeAsync();
        }

        [Command("skip", RunMode = RunMode.Async)]
        [Summary("Next track")]
        public async Task SkipAsync()
        {
            if (!_service.HasPlayer(StaticResources.CurrentGuildId))
                return;
            _player = await GetPlayer();
            await _player.SkipAsync();
        }

        private void GetIds()
        {
            StaticResources.CurrentGuildId = Context.Guild.Id;
            StaticResources.CurrentVoiceChannelId = (Context.User as IVoiceState).VoiceChannel.Id;
        }

        private async Task<QueuedLavalinkPlayer> GetPlayer()
        {
            GetIds();
            if (_player != null)
                return _player;
            Console.WriteLine("Player is null - creating new Player!");
            var player = _service.GetPlayer<QueuedLavalinkPlayer>(StaticResources.CurrentGuildId)
                ?? await _service.JoinAsync<QueuedLavalinkPlayer>(StaticResources.CurrentGuildId, (Context.User as IVoiceState).VoiceChannel.Id);
            return player;
        }
    }
}
