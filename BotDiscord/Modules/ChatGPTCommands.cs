using BotDiscord.Common;
using Discord;
using Discord.Commands;
using OpenAI.Chat;

namespace BotDiscord.Modules
{
    public sealed class ChatGPTCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ctm", RunMode = RunMode.Async)]
        public async Task ChatGPT([Remainder] string? texto)
        {
            ChatCompletion resp = await ObterCompletionCreateResponse(texto);
            await Context.Message.ReplyAsync(text: resp.Content[0].Text);
        }

        private static async Task<ChatCompletion> ObterCompletionCreateResponse(string? texto)
        {
            ChatClient client = new(model: StaticKeys.Model, credential: StaticKeys.ChatGPTApiKey);
            ChatCompletion resp = await client.CompleteChatAsync(texto);

            return resp;
        }
    }
}