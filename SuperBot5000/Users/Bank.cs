using System;
using System.Collections.Generic;
using System.Text;

namespace SuperBot5000.Users
{
    public class Bank
    {
        public long GamblingEarned { get; set; }
        public long GamblingSpent { get; set; }
        public long DailiesEarned { get; set; }
        public long ActivityEarned { get; set; }
        public long ShopSpent { get; set; }

        public Bank()
        {
        }

        public long GetTotalEarned() => GamblingEarned + DailiesEarned + ActivityEarned;
        public long GetTotalSpent() => GamblingSpent + ShopSpent;
    }
}
