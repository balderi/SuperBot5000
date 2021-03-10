using System.Threading.Tasks;
using Lavalink4NET.Player;
using Lavalink4NET.Events;
using Lavalink4NET;

namespace SuperBot5000.Music
{
    internal sealed class SuperPlayer : QueuedLavalinkPlayer
    {
        public override async Task OnTrackStartedAsync(TrackStartedEventArgs e)
        {
            await base.OnTrackStartedAsync(e);
        }
    }
}
