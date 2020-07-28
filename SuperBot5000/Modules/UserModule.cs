using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SuperBot5000.Users;
using System.Linq;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Get info about the user calling.")]
        public async Task InfoAsync(string name = null)
        {
            User user;
            var du = Context.User as SocketGuildUser;
            var role = (du as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Admin");
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (du.Roles.Contains(role))
                {
                    UserList.GetUserList().TryGetUserByName(name, out user);
                    if (user == null)
                    {
                        await ReplyAsync("That user does not exist yet... a user will be created automatically when they interact with me.");
                        return;
                    }
                }
                else
                {
                    await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                    return;
                }
            }
            else
            {
                user = UserList.GetUserList().GetUser(Context.User);
            }
            
            await ReplyAsync(
                $"User info:\n" +
                $"Name: {user.Name}\n" +
                $"Balance: {user.GetBalance()} coins\n" +
                $"Last played slots {user.GetFormattedLastPlayed()}\n" +
                $"Daily coins available: {user.CanGetCoins()}\n" +
                $"Activity points: {user.OnlinePoints}"
            );
        }

        [Command("bank")]
        [Summary("Get user's bank.")]
        public async Task BankAsync()
        {
            User user = UserList.GetUserList().GetUser(Context.User);
            await ReplyAsync(
                $"{user.Name}'s Bank:\n" +
                $"Spent on slots: {user.Bank.GamblingSpent:N0} coins\n" +
                $"Won on slots: {user.Bank.GamblingEarned:N0} coins\n" +
                $"Earned from dailies: {user.Bank.DailiesEarned:N0} coins\n" +
                $"Erned from activity: {user.Bank.ActivityEarned:N0} coins\n\n" +
                $"Total income: {user.Bank.GetTotalEarned():N0} coins\n" +
                $"Total expense: {user.Bank.GetTotalSpent():N0} coins\n" +
                $"Cashflow: {(user.Bank.GetTotalEarned() - user.Bank.GetTotalSpent()):N0} coins"
            );
        }

        [Command("balance")]
        [Summary("Get user's coin balance.")]
        public async Task BalanceAsync()
        {
            User user = UserList.GetUserList().GetUser(Context.User);
            await ReplyAsync($"{user.Name}'s balance is {user.GetBalance()} coins.");
        }

        [Command("daily")]
        [Summary("Recieve daily coins.")]
        public async Task DailyAsync()
        {
            User user = UserList.GetUserList().GetUser(Context.User);
            if(!user.CanGetCoins())
            {
                await ReplyAsync($"No can do, {user.Name}...");
            }
            else
            {
                user.GetDailyCoins();
                await ReplyAsync($"You got 100 coins!");
            }
        }
    }
}
