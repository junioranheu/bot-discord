using BotDiscord.Common;
using BotDiscord.Models;
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
            CompletionCreateResponse respChatGPT = await ObterCompletionCreateResponse(texto);
            ChatGPTResponse chatGPTResponse = ObterChatGPTResponse(respChatGPT);

            if (!chatGPTResponse.Successful)
            {
                await Logger.Log(LogSeverity.Critical, $"{nameof(ChatGPTCommands)}", chatGPTResponse?.Error?.Message ?? string.Empty);

                string erro = !string.IsNullOrEmpty(chatGPTResponse?.Error?.Message) ? $": {chatGPTResponse.Error.Message}" : string.Empty;
                await Context.Message.ReplyAsync($"Ops, {Context.User.Username}! Parece que houve um erro{erro}");
                return;
            }

            foreach (var choice in chatGPTResponse!.Choices)
            {
                //await Context.Message.ReplyAsync(choice.Text);
            }
        }

        private static async Task<CompletionCreateResponse> ObterCompletionCreateResponse(string? texto)
        {
            OpenAIService gpt3 = new(new OpenAiOptions()
            {
                ApiKey = apiKey
            });

            CompletionCreateResponse respChatGPT = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = texto ?? string.Empty,
                Model = OpenAI.GPT3.ObjectModels.Models.TextDavinciV2,
                Temperature = 0.5F,
                MaxTokens = 100,
                N = 1
            });

            return respChatGPT;
        }

        private static ChatGPTResponse ObterChatGPTResponse(CompletionCreateResponse respChatGPT)
        {
            ChatGPTResponse response = new()
            {
                Choices = respChatGPT.Choices,
                CreatedAt = respChatGPT.CreatedAt,
                Error = respChatGPT.Error,
                Id = respChatGPT.Id,
                Model = respChatGPT.Model,
                Successful = respChatGPT.Successful
            };

            return response;
        }
    }
}