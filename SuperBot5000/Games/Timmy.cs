using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SuperBot5000.Games
{
    public static class Timmy
    {
        private static readonly Random _rnd = new();
        private static readonly List<TimmyPoem> _timmyPoems = JsonConvert.DeserializeObject<List<TimmyPoem>>(File.ReadAllText("timmy.json"));

        public static TimmyPoem GetPoem()
        {
            return _timmyPoems[_rnd.Next(0, _timmyPoems.Count)];
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
