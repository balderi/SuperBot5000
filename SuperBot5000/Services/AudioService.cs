using Lavalink4NET;
using Lavalink4NET.Events;
using Lavalink4NET.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Services
{
    public class AudioService
    {
        public List<LavalinkTrack> TrackQueue { get; } = new List<LavalinkTrack>();

        private bool _isInitialized;

        public void Init(IAudioService service)
        {
            if (_isInitialized)
                return;

            try { service.TrackStarted -= TrackStarted; }
            finally { service.TrackStarted += TrackStarted; }

            try { service.TrackEnd -= TrackEnded; }
            finally { service.TrackEnd += TrackEnded; }
            _isInitialized = true;
            Console.WriteLine("*** Initialized AudioService");
        }

        public LavalinkTrack GetNextTrack()
        {
            TrackQueue.RemoveAt(0);
            return TrackQueue.FirstOrDefault();
        }

        public LavalinkTrack GetFirstTrack()
        {
            return TrackQueue.FirstOrDefault();
        }

        public void RemoveFirstTrack()
        {
            TrackQueue.RemoveAt(0);
        }

        public void AddToQueue(LavalinkTrack track)
        {
            TrackQueue.Add(track);
        }

        public static string PrintQueue(QueuedLavalinkPlayer player)
        {
            if (player.Queue.IsEmpty)
                return "";
            StringBuilder sb = new StringBuilder($"Current queue:\n> ► `{player.CurrentTrack.Title}`\n");
            int i = 1;
            foreach(var t in player.Queue)
            {
                sb.AppendLine($"> {i++}: `{t.Title}`");
            }
            return sb.ToString();
        }

        public static async Task TrackStarted(object sender, TrackStartedEventArgs e)
        {
            var player = e.Player as QueuedLavalinkPlayer;

            if (player.CurrentTrack.Title != null)
            {
                Console.WriteLine($"[TrackStarted] Queue has {player.Queue.Count} items");
                //await ReplyAsync($"Now playing {e.Player.CurrentTrack.Title}!");
            }
            else
            {
                Console.WriteLine("Track title was null on track start");
            }
            await Task.CompletedTask;
        }

        public static async Task TrackEnded(object sender, TrackEndEventArgs e)
        {
            var player = e.Player as QueuedLavalinkPlayer;
            await Task.CompletedTask;
        }
    }
}
