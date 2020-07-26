using Discord;
using Discord.Commands;
using SuperBot5000.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("coins")]
        [Summary("($A) Adjust a user's coins balance.")]
        public async Task CoinsAsync(string type, string userName, long value)
        {
            if(!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            var userList = UserList.GetUserList();
            if (!userList.TryGetUserByName(userName, out User user))
            {
                await ReplyAsync("That user does not exist.");
                return;
            }
            switch (type)
            {
                default:
                case "give":
                    user.AddCoins(value);
                    await ReplyAsync($"Gave {Context.User.Mention} {value} coins!");
                    break;
                case "take":
                    try
                    {
                        user.SubtractCoins(value);
                        await ReplyAsync($"Took {value} coins from {Context.User.Mention}!");
                    }
                    catch(ArithmeticException e)
                    {
                        await ReplyAsync($"Error: {e.Message}");
                    }
                    break;
                case "set":
                    user.Balance = value;
                    await ReplyAsync($"Set {Context.User.Mention}'s balance to {value} coins!");
                    break;
            }
        }
    }
}
