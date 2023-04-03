using BotDiscord.Common;
using Discord;
using Discord.Commands;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace BotDiscord.Modules
{
    public sealed class ChatGPTCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ctm", RunMode = RunMode.Async)]
        public async Task ChatGPT([Remainder] string? texto)
        {
            OpenAIService gpt3 = new(new OpenAiOptions()
            {
                ApiKey = "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"
            });

            CompletionCreateResponse resultado = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = texto,
                Model = OpenAI.GPT3.ObjectModels.Models.TextDavinciV2,
                Temperature = 0.5F,
                MaxTokens = 100,
                N = 3
            });

            if (!resultado.Successful)
            {
                await Logger.Log(LogSeverity.Info, $"{nameof(ChatGPTCommands)}", resultado?.Error?.Message ?? string.Empty);
                string erro = !string.IsNullOrEmpty(resultado?.Error?.Message) ? $": {resultado.Error.Message}" : string.Empty;
                await Context.Message.ReplyAsync($"Ops, {Context.User.Username}! Parece que houve um erro{erro}");
            }

            foreach (var choice in resultado!.Choices)
            {
                //await Context.Message.ReplyAsync(choice.Text);
            }
        }
    }
}