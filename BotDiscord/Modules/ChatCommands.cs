using Discord;
using Discord.Commands;

namespace BotDiscord.Modules
{
    public class ChatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("hello", RunMode = RunMode.Async)]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
        }
    }
}