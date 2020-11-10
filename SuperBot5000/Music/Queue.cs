using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Music
{
    public class Queue
    {
        public static List<string> CurrentQueue = new List<string>();
        public static string CurrentMusic;

        public static void AddMusic(string filename)
        {
            CurrentQueue.Add(filename);
        }

        public static Task RemoveMusic(int index = 0)
        {
            CurrentQueue.RemoveAt(index);
            return Task.CompletedTask;
        }

        public static async Task AdvanceQueueAsync()
        {
            await RemoveMusic();
            CurrentMusic = CurrentQueue.FirstOrDefault();
        }
    }
}
