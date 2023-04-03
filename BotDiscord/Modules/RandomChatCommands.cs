using BotDiscord.Common;
using Discord;
using Discord.Commands;

namespace BotDiscord.Modules
{
    public sealed class RandomChatCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ola", RunMode = RunMode.Async)]
        public async Task Ola()
        {
            await Context.Message.ReplyAsync(text: $"Olá, {Context.User.Username}. Tudo certo?");
        }

        [Command("mia", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = Consts.SemPermissao)]
        public async Task Mia()
        {
            var embedBuilder = new EmbedBuilder { Color = new Color(255, 0, 0), ImageUrl = "https://media0.giphy.com/media/3o7aDdyoaef41qkn9m/200.gif" };
            Embed embed = embedBuilder.Build();
            await Context.Message.ReplyAsync(text: "Fino, huh? 🗿🍷", embed: embed);
        }
    }
}