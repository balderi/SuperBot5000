using Discord;
using Discord.Commands;
using SuperBot5000.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("wtf")]
        [Summary("($A) test command; please ignore")]
        public async Task WtfAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            await ReplyAsync("wat?");
        }

        [Command("latest")]
        [Summary("($A) test command; please ignore")]
        public async Task LatestAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "show -s --format='%h%n%an <%ae>%n%ad%n%s'",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string output = "";
            while (!proc.StandardOutput.EndOfStream)
            {
                output += proc.StandardOutput.ReadLine();
            }

            await ReplyAsync(StaticResources.GitFormat(output));
        }

        [Command("pull")]
        [Summary("($A) Pull and build the latest commit")]
        public async Task PullAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            await ReplyAsync("Pulling the latest commit...");
            Process.Start("../../../../../../buildnrun.sh");
        }

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
                    user.SetBalance(value);
                    await ReplyAsync($"Set {Context.User.Mention}'s balance to {value} coins!");
                    break;
                default:
                    await ReplyAsync($"usage: `!coins [give|take|set] <@user> <value>`");
                    break;
            }
        }
    }
}
