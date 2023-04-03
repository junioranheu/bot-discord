using Discord;
using Discord.Commands;

namespace BotDiscord.Modules
{
    public sealed class RandomChatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ola", RunMode = RunMode.Async)]
        public async Task Ola()
        {
            await Context.Message.ReplyAsync($"Olá, {Context.User.Username}. Tudo certo?");
        }
    }
}