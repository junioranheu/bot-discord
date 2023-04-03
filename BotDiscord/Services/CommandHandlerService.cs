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
            // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Bootstrapper.ServiceProvider);

            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommand;

            _commands.CommandExecuted += async (optional, context, result) =>
            {
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    // the command failed, let's notify the user that something happened.
                    await context.Channel.SendMessageAsync($"error: {result}");
                }
            };

            foreach (var module in _commands.Modules)
            {
                await Logger.Log(LogSeverity.Info, $"{nameof(CommandHandlerService)} | Commands", $"Module '{module.Name}' initialized.");
            }
        }

        private async Task HandleCommand(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            if (arg is not SocketUserMessage msg)
            {
                return;
            }

            // We don't want the bot to respond to itself or other bots.
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
                return;

            // Create a Command Context.
            var context = new SocketCommandContext(_client, msg);

            int markPos = 0;
            if (msg.HasCharPrefix('!', ref markPos) || msg.HasCharPrefix('?', ref markPos))
            {
                await _commands.ExecuteAsync(context, markPos, Bootstrapper.ServiceProvider);
            }
        }
    }
}