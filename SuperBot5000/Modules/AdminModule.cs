using Discord;
using Discord.Commands;
using SuperBot5000.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperBot5000.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("emojirefresh")]
        [Summary("($A) Refreshes the list of emoji")]
        public async Task EmojiRefreshAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            foreach(var f in Directory.GetFiles("emoji"))
            {
                var file = Path.GetFileNameWithoutExtension(f);
                if (StaticResources.GetRole(Context, new string[] { $"{file}emoji" }).Count() < 1)
                    await Context.Guild.CreateRoleAsync($"{file}emoji");
                if (!(Context.Guild.Emotes.Select(x => x.Name == file).Count() > 0))
                    await Context.Guild.CreateEmoteAsync(file, new Image(f),
                    new Optional<IEnumerable<IRole>>(StaticResources.GetRole(Context, new string[] { "Bot", $"{file}emoji" })));
            }

            await ReplyAsync("Refreshed emoji!");
        }

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

        [Command("roles")]
        [Summary("($A) list roles and ids")]
        public async Task RolesAsync()
        {
            if (!StaticResources.ValidateAdminUser(Context))
            {
                await ReplyAsync($"I'm sorry, {Context.User.Mention}; I'm afraid I can't let you do that.");
                return;
            }

            var sb = new StringBuilder("Roles:\n");
            foreach(var r in Context.Guild.Roles)
            {
                sb.AppendLine($"{r.Name} - {r.Id}");
            }

            await ReplyAsync(sb.ToString());
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
                    Arguments = "log -1 --format=%h%n%aN%x20%x3c%ae%x3e%n%ad%n%s",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            var output = new StringBuilder();
            while (!proc.StandardOutput.EndOfStream)
            {
                output.AppendLine(proc.StandardOutput.ReadLine());
            }

            await ReplyAsync(StaticResources.GitFormat(output.ToString()));
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

            try
            {
                var sw = File.CreateText("pullmyfile");
                sw.WriteLine(Context.Channel.Id);
                sw.Close();
            }
            catch(Exception e)
            {
                await ReplyAsync(e.Message);
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
                    await ReplyAsync($"Gave {user.Name} {value} coins!");
                    break;
                case "take":
                    try
                    {
                        user.SubtractCoins(value);
                        await ReplyAsync($"Took {value} coins from {user.Name}!");
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
