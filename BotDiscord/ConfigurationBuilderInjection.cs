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
        public static ConfigurationBuilderInjectionResponse AddConfigurationBuilder(IConfigurationRoot config)
        {
            Bootstrapper.Init();

            DiscordSocketClient client = AddClient();
            AddCommands();
            AddConfig(config);
            string token = GetToken(config);

            AddTypes();

            return GetConfigurationBuilderInjectionResponse(token, client);
        }

        private static DiscordSocketClient AddClient()
        {
            DiscordSocketConfig clientConfig = new()
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
            };

            DiscordSocketClient client = new(clientConfig);

            Bootstrapper.RegisterInstance(client);

            return client;
        }

        private static void AddCommands()
        {
            CommandService commands = new(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            Bootstrapper.RegisterInstance(commands);
        }

        private static void AddConfig(IConfigurationRoot config)
        {
            Bootstrapper.RegisterInstance(config);
        }

        private static string GetToken(IConfigurationRoot config)
        {
            string token = config["token"] ?? string.Empty; // secrets.json;

            return token;
        }

        private static void AddTypes()
        {
            Bootstrapper.RegisterType<ICommandHandler, CommandHandler>();
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