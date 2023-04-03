using BotDiscord.Common;
using BotDiscord.Init;
using BotDiscord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder().
             AddJsonFile("appsettings.json").
             AddEnvironmentVariables().
             AddUserSecrets<Program>().
             Build();

string token = config["secretDiscordBotToken"] ?? string.Empty; // secrets.json;

var commands = new CommandService(new CommandServiceConfig
{
    LogLevel = LogSeverity.Info,
    CaseSensitiveCommands = false
});

var clientConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
};

var client = new DiscordSocketClient(clientConfig);

// Setup your DI container.
Bootstrapper.Init();
Bootstrapper.RegisterInstance(client);
Bootstrapper.RegisterInstance(commands);
Bootstrapper.RegisterInstance(config);
Bootstrapper.RegisterType<ICommandHandler, CommandHandler>();

await Main(token);

async Task Main(string token)
{
    await Bootstrapper.ServiceProvider!.GetRequiredService<ICommandHandler>().InitializeAsync();

    client.Ready += async () =>
    {
        await Logger.Log(LogSeverity.Info, "Ready", $"Number {Guid.NewGuid} is connected and ready!");
    };

    // Login and connect.
    if (string.IsNullOrWhiteSpace(token))
    {
        await Logger.Log(LogSeverity.Error, $"{nameof(Program)} | {nameof(Main)}", "Token is null or empty.");
        return;
    }

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    // Wait infinitely so your bot actually stays connected.
    await Task.Delay(Timeout.Infinite);
}