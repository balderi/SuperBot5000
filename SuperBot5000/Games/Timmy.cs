using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SuperBot5000.Games
{
    public static class Timmy
    {
        private static readonly Random rnd = new Random(DateTime.Now.Millisecond);
        private static readonly List<TimmyPoem> timmyPoems = JsonConvert.DeserializeObject<List<TimmyPoem>>(File.ReadAllText("timmy.json"));

        public static TimmyPoem GetPoem()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var poem = new TimmyPoem();
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            return timmyPoems[rnd.Next(0, timmyPoems.Count)];
        }
    }

    public class TimmyPoem
    {
        public string Body { get; set; }
        public string Link { get; set; }

        public TimmyPoem()
        {

        }
    }
}
