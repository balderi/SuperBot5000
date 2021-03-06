﻿using Discord.WebSocket;
using System;

namespace SuperBot5000.Users
{
    public class User
    {
        public string Name { get; set; }
        public string HumanName { get; set; }
        public long Balance { get; set; }
        public DateTime LastPlayed { get; set; }
        public DateTime LastGotCoins { get; set; }
        public int OnlinePoints { get; set; }
        public Bank Bank { get; set; }

        public User()
        {
            //nothing to do here...
        }

        public User(SocketUser user)
        {
            Name = user.Mention;
            HumanName = user.Username;
            Balance = 100;
            LastPlayed = DateTime.MinValue;
            OnlinePoints = 0;
            Bank = new Bank()
            {
                GamblingEarned = 0,
                GamblingSpent = 0,
                ActivityEarned = 0,
                DailiesEarned = 100,
                ShopSpent = 0
            };
        }

        public void AddCoins(long value)
        {
            if(value < 0)
            {
                SubtractCoins(Math.Abs(value));
                return;
            }
            if (Balance + value < Balance)
                Balance = long.MaxValue;
            else
                Balance += value;
            UserList.GetUserList().SaveList();
        }

        public void SubtractCoins(long value)
        {
            if (value < 0)
            {
                AddCoins(Math.Abs(value));
                return;
            }
            if (Balance - value < 0)
                throw new ArithmeticException("Value would make balance negative.");
            else
                Balance -= value;
            UserList.GetUserList().SaveList();
        }

        public string GetBalance()
        {
            return Balance.ToString("N0");
        }

        public void SetBalance(long value)
        {
            Balance = value;
            UserList.GetUserList().SaveList();
        }

        public TimeSpan GetLastPlayed()
        {
            return DateTime.Now.Subtract(LastPlayed);
        }

        public string GetFormattedLastPlayed()
        {
            var ts = GetLastPlayed();
            if(ts.Days > 1000)
            {
                return "never";
            }
            else if (ts.Days > 0)
            {
                return ts.Days == 1 ? $"about a day ago" : $"about {ts.Days} days ago";
            }
            else if (ts.Hours > 0)
            {
                return ts.Hours == 1 ? $"about an hour ago" : $"about {ts.Hours} hours ago";
            }
            else if (ts.Minutes > 0)
            {
                return ts.Minutes == 1 ? $"about a minute ago" : $"about {ts.Minutes} minutes ago";
            }
            else if (ts.Seconds > 0)
            {
                return ts.Seconds == 1 ? $"about a second ago" : $"about {ts.Seconds} seconds ago";
            }
            else
            {
                return "less than a second ago";
            }
        }

        public bool CanGetCoins()
        {
            return DateTime.Now.Subtract(LastGotCoins).TotalDays > 1;
        }

        public void GetDailyCoins()
        {
            LastGotCoins = DateTime.Now;
            AddCoins(100);
            Bank.DailiesEarned += 100;
        }

        public void IncrementOLPoints()
        {
            OnlinePoints++;
        }

        public void TryRedeemOLPoints()
        {
            if(OnlinePoints >= 100)
            {
                OnlinePoints = 0;
                AddCoins(100);
                Bank.ActivityEarned += 100;
            }
        }
    }
}
