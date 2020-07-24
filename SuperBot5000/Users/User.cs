using System;

namespace SuperBot5000.Users
{
    public class User
    {
        public string Name { get; set; }
        public int Balance { get; set; }
        public DateTime LastPlayed { get; set; }
        public DateTime LastGotCoins { get; set; }

        public User()
        {
            //nothing to do here...
        }

        public User(string name)
        {
            Name = name;
            Balance = 100;
            LastPlayed = DateTime.MinValue;
        }

        public void AddCoins(int value)
        {
            if(value < 0)
            {
                SubtractCoins(Math.Abs(value));
                return;
            }
            if (Balance + value < Balance)
                Balance = int.MaxValue;
            else
                Balance += value;
            UserList.GetUserList().SaveList();
        }

        public void SubtractCoins(int value)
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
                return ts.Days == 1 ? $"about a day" : $"about {ts.Days} days ago";
            }
            else if (ts.Hours > 0)
            {
                return ts.Hours == 1 ? $"about an hour" : $"about {ts.Hours} hours ago";
            }
            else if (ts.Minutes > 0)
            {
                return ts.Minutes == 1 ? $"about a minute" : $"about {ts.Minutes} minutes ago";
            }
            else if (ts.Seconds > 0)
            {
                return ts.Seconds == 1 ? $"about a second" : $"about {ts.Seconds} seconds ago";
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

        public void GetCoins()
        {
            LastGotCoins = DateTime.Now;
            AddCoins(100);
        }
    }
}
