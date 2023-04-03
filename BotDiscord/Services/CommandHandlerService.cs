using BotDiscord.Common;
using BotDiscord.Init;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace BotDiscord.Services
{
    public sealed class CommandHandlerService : ICommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public CommandHandlerService(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
        }

        public async Task Initialize()
        {
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Bootstrapper.ServiceProvider);

            _client.MessageReceived += HandleCommand;

            _commands.CommandExecuted += async (optional, context, result) =>
            {
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync($"Erro no comando: {result}");
                    await Logger.Log(LogSeverity.Error, $"{nameof(CommandHandlerService)} | Commands", $"{result}");
                }
            };

            foreach (var module in _commands.Modules)
            {
                await Logger.Log(LogSeverity.Info, $"{nameof(CommandHandlerService)} | Commands", $"O módulo '{module.Name}' foi inicializado");
            }
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            if (arg is not SocketUserMessage msg)
            {
                return;
            }

            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
            {
                return;
            }

            SocketCommandContext context = new(_client, msg);

            int markPos = 0;
            if (msg.HasCharPrefix('!', ref markPos))
            {
                await _commands.ExecuteAsync(context, markPos, Bootstrapper.ServiceProvider);
            }
        }
    }
}