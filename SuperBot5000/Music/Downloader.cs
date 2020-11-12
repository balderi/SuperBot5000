using System.Diagnostics;
using System.Threading.Tasks;

namespace SuperBot5000.Music
{
    public class Downloader
    {
        public static async Task YoutubeDownloadAsync(string address)
        {
            using(var ytdl = CreateProcess(address))
            {
                try
                {
                    await ytdl.StandardOutput.ReadToEndAsync();
                }
                catch { }
            }
        }

        private static Process CreateProcess(string address)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = $"-c -o \"music/%(title)s.%(ext)s\" -x --audio-format mp3 {address}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
