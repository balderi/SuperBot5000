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
                }
                else
                {
                    await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                    return;
                }
            }
            else
            {
                user = UserList.GetUserList().GetUser(Context);
            }
            
            await ReplyAsync(
                $"User info:\nName: {user.Name}\n" +
                $"Balance: {user.GetBalance()} coins\n" +
                $"Last played slots {user.GetFormattedLastPlayed()}\n" +
                $"Daily coins available: {user.CanGetCoins()}"
            );
        }

        [Command("balance")]
        [Summary("Get user's coin balance.")]
        public async Task BalanceAsync()
        {
            User user = UserList.GetUserList().GetUser(Context);
            await ReplyAsync($"{user.Name}'s balance is {user.GetBalance()} coins.");
        }

        [Command("daily")]
        [Summary("Recieve daily coins.")]
        public async Task DailyAsync()
        {
            User user = UserList.GetUserList().GetUser(Context);
            if(!user.CanGetCoins())
            {
                await ReplyAsync($"No can do, {user.Name}...");
            }
            else
            {
                user.GetCoins();
                await ReplyAsync($"You got 100 coins!");
            }
        }
    }
}
