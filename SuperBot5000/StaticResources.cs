﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SuperBot5000
{
    public class StaticResources
    {
        static Random rnd ;
        static List<string> memes;

        static string[] slotsEmoji;

        public static void Init()
        {
            rnd = new Random(DateTime.Now.Millisecond);
            memes = Directory.GetFiles("memes").ToList();
            slotsEmoji = new string[]
            {
                "🍇", "🍌", "🍒", "❤️", "🔔", "🏆", "🎺", "💎", "💰", "👑", "⚜️", "⭐", "🎲", "🍀", "🍋", "7️⃣", "🙂"
            };
        }

        public static string GetRandomMemePath() => memes[rnd.Next(0, memes.Count)];

        public static int GetRandomSlotsValue() => rnd.Next(0, slotsEmoji.Length);

        public static string GetSlotsEmoji(int id) => id >= 0 && id < slotsEmoji.Length ? slotsEmoji[id] : "Error!";
    }
}