using Discord;
using Discord.Commands;
using SuperBot5000.Users;
using System;
using System.Text;

namespace SuperBot5000.Games
{
    public class Slots
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        public static Embed Play(SocketCommandContext context, long bet)
        {
            var retval = new EmbedBuilder()
            {
                Title = $"SuperSlots5000"
            };

            User user = UserList.GetUserList().GetUser(context);

            if(user.Balance < bet)
            {
                retval.AddField("Error!", $"You cannot afford!");
                return retval.Build();
            }

            user.SubtractCoins(bet);

            user.LastPlayed = DateTime.Now;

            var vals = GetProperSlots();

            retval.WithDescription($"{StaticResources.GetSlotsEmoji(vals.Item1)} | {StaticResources.GetSlotsEmoji(vals.Item2)} | {StaticResources.GetSlotsEmoji(vals.Item3)}");

            long winnings = -bet;

            string foot;

            if (vals.Item1 == vals.Item2 && vals.Item2 == vals.Item3)
            {
                winnings += (bet * 100) + bet;
                user.AddCoins(winnings);
                foot = $"A winner is you! (+{winnings:N0} coins)";
            }
            else if(vals.Item1 == 7) //diamond in first slot
            {
                winnings += (bet * 10) + bet;
                user.AddCoins(winnings);
                foot = $"{StaticResources.GetSlotsEmoji(7)} in first slot! (+{winnings:N0} coins)";
            }
            else if (vals.Item1 == vals.Item2 || vals.Item1 == vals.Item3 || vals.Item2 == vals.Item3)
            {
                winnings += (bet * 5) + bet;
                user.AddCoins(winnings);
                foot = $"Two of a kind! (+{winnings:N0} coins)";
            }
            else
            {
                foot = $"Better luck next time! ({winnings:N0} coins)";
            }

            retval.WithFooter($"{foot}\nYour balance is {user.GetBalance()} coins");

            return retval.Build();
        }

        public static Embed PlayMulti(SocketCommandContext context, int times, long bet)
        {
            var multiBet = times * bet;

            var retval = new EmbedBuilder()
            {
                Title = $"SuperSlots5000 - Multi"
            };

            User user = UserList.GetUserList().GetUser(context);

            if (user.Balance < multiBet)
            {
                retval.AddField("Error!", $"You cannot afford!");
                return retval.Build();
            }

            long winnings = 0;

            user.LastPlayed = DateTime.Now;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < times; i++)
            {
                var vals = GetProperSlots();

                var roundResult = -bet;

                if (vals.Item1 == vals.Item2 && vals.Item2 == vals.Item3)
                {
                    roundResult += (bet * 100) + bet;
                }
                else if (vals.Item1 == 7) //diamond in first slot
                {
                    roundResult += (bet * 10) + bet;
                }
                else if (vals.Item1 == vals.Item2 || vals.Item1 == vals.Item3 || vals.Item2 == vals.Item3)
                {
                    roundResult += (bet * 5) + bet;
                }

                winnings += roundResult;

                sb.AppendLine($"{StaticResources.GetSlotsEmoji(vals.Item1)} | {StaticResources.GetSlotsEmoji(vals.Item2)} | {StaticResources.GetSlotsEmoji(vals.Item3)} ឵឵ ឵឵ ឵឵ ឵឵({roundResult:N0})");
            }

            user.AddCoins(winnings);

            retval.WithDescription(sb.ToString());

            retval.WithFooter($"Total result of multislots: {winnings:N0} coins\nYour balance is {user.GetBalance()} coins");

            return retval.Build();
        }

        public static Tuple<int, int, int> GetProperSlots()
        {
            int val1 = StaticResources.GetRandomSlotsValue();
            int val2 = StaticResources.GetRandomSlotsValue();
            int val3 = StaticResources.GetRandomSlotsValue();

            if(val1 == 7 && rnd.Next(0, 3) > 0)
                val1 = (val1 + StaticResources.GetRandomSlotsValue()) % StaticResources.GetTotalSlots();

            if ((val1 == val2) && rnd.Next(0, 3) == 0)
                val2 = (val2 + StaticResources.GetRandomSlotsValue()) % StaticResources.GetTotalSlots();

            if ((val2 == val3 || val1 == val3) && rnd.Next(0, 3) == 0)
                val3 = (val3 + StaticResources.GetRandomSlotsValue()) % StaticResources.GetTotalSlots();

            return new Tuple<int, int, int>(val1, val2, val3);
        }

        public static Embed Help()
        {
            var retval = new EmbedBuilder()
            {
                Title = "SuperSlots5000 - Help",
                Description = "Super basic slots game.",
                Color = Color.Gold
            };

            retval.AddField("Rules", 
                $" • You bet coins to play - default is 10, use `slots <bet>` to bet a different amount, e.g. `slots 50`\n" +
                $" • Three of a kind pays bet x 100\n" +
                $" • Two of a kind pays bet x 5\n" +
                $" • A `{StaticResources.GetSlotsEmoji(7)}` in the first slot pays bet x 10\n\n" +
                $"Only the highest paying prize is paid out. Winning will refund your bet on top of your winnings.");
            retval.AddField("Examples",
                $"With a bet of 10 coins\n\n" +
                $"`{StaticResources.GetSlotsEmoji(14)} | {StaticResources.GetSlotsEmoji(14)} | {StaticResources.GetSlotsEmoji(14)}`\n" +
                $"Pays 1000 coins (`{StaticResources.GetSlotsEmoji(14)}` in all three slots)\n\n" +
                $"`{StaticResources.GetSlotsEmoji(2)} | {StaticResources.GetSlotsEmoji(4)} | {StaticResources.GetSlotsEmoji(2)}`\n" +
                $"Pays 50 coins (`{StaticResources.GetSlotsEmoji(2)}` in two slots)\n\n" +
                $"`{StaticResources.GetSlotsEmoji(7)} | {StaticResources.GetSlotsEmoji(12)} | {StaticResources.GetSlotsEmoji(12)}`\n" +
                $"Pays 100 coins (`{StaticResources.GetSlotsEmoji(7)}` in the first slot - the two `{StaticResources.GetSlotsEmoji(12)}` are ignored, as two of a kind pays less)\n\n" +
                $"`{StaticResources.GetSlotsEmoji(0)} | {StaticResources.GetSlotsEmoji(7)} | {StaticResources.GetSlotsEmoji(2)}`\n" +
                $"Pays nothing (you lose... good day, sir!)");
            retval.AddField("Multislots",
                $"Use `multislots <times> <bet>` to play, e.g. `multislots 10 50` to play 10 games with a bet of 50 coins each.\n" +
                $"Default is `multislot 3 10.`");
            return retval.Build();
        }
    }
}
