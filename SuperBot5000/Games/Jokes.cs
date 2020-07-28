using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SuperBot5000.Games
{
    public static class Jokes
    {
        private static readonly Random rnd = new Random(DateTime.Now.Millisecond);
        private static readonly List<RedditJoke> redditJokes = JsonConvert.DeserializeObject<List<RedditJoke>>(File.ReadAllText("reddit_jokes.json"));

        public static RedditJoke GetJoke()
        {
            return redditJokes[rnd.Next(0, redditJokes.Count)];
        }
    }

    public class RedditJoke
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public int Score { get; set; }
        public string Body { get; set; }

        public RedditJoke()
        {

        }
    }
}
