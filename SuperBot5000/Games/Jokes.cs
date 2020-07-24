using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SuperBot5000.Games
{
    public static class Jokes
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static List<RedditJoke> redditJokes = JsonConvert.DeserializeObject<List<RedditJoke>>(File.ReadAllText("reddit_jokes.json"));

        public static RedditJoke GetJoke()
        {
            return redditJokes[rnd.Next(0, redditJokes.Count)];
        }
    }

    public class RedditJoke
    {
        public string title { get; set; }
        public string id { get; set; }
        public int score { get; set; }
        public string body { get; set; }

        public RedditJoke()
        {

        }
    }
}
