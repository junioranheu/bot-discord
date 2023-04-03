using Discord;
using Discord.Commands;

namespace BotDiscord.Modules
{
    public sealed class ChatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ola", RunMode = RunMode.Async)]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Olá, {Context.User.Username}. Tudo certo?");
        }
    }
}