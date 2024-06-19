using BotDiscord.Common;
using BotDiscord.Init;
using BotDiscord.Models;
using BotDiscord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace BotDiscord
{
    public class ConfigurationBuilderInjection
    {
        public static async Task<ConfigurationBuilderInjectionResponse> AddConfigurationBuilder(IConfigurationRoot config)
        {
            Bootstrapper.Init();

            DiscordSocketClient client = AddClient();
            AddCommands();
            AddConfig(config);
            string token = await GetToken(config);
            await AddChatGPTApiKey(config);
            AddServices();

            return GetConfigurationBuilderInjectionResponse(token, client);
        }

        private static DiscordSocketClient AddClient()
        {
            DiscordSocketConfig clientConfig = new()
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
            };

            DiscordSocketClient client = new(clientConfig);

            Bootstrapper.RegistrarInstancia(client);

            return client;
        }

        private static void AddCommands()
        {
            CommandService commands = new(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            Bootstrapper.RegistrarInstancia(commands);
        }

        private static void AddConfig(IConfigurationRoot config)
        {
            Bootstrapper.RegistrarInstancia(config);
        }

        private static async Task<string> GetToken(IConfigurationRoot config)
        {
            string token = config["discord"] ?? string.Empty; // secrets.json;

            if (!string.IsNullOrEmpty(token))
            {
                await Logger.Log(LogSeverity.Info, $"{nameof(ConfigurationBuilderInjection)}", "Token do Discord em mãos");
            }

            return token;
        }

        private static async Task AddChatGPTApiKey(IConfigurationRoot config)
        {
            string chatGPTApiKey = config["chatgpt"] ?? string.Empty; // secrets.json;
            StaticKeys.ChatGPTApiKey = chatGPTApiKey;

            if (!string.IsNullOrEmpty(chatGPTApiKey))
            {
                await Logger.Log(LogSeverity.Info, $"{nameof(ConfigurationBuilderInjection)}", "Chave da API do ChatGPT em mãos");
            }
        }

        private static void AddServices()
        {
            Bootstrapper.RegistrarService<ICommandHandlerService, CommandHandlerService>();
        }

        private static ConfigurationBuilderInjectionResponse GetConfigurationBuilderInjectionResponse(string token, DiscordSocketClient client)
        {
            ConfigurationBuilderInjectionResponse configInjection = new()
            {
                Token = token,
                Client = client
            };

            return configInjection;
        }
    }
}