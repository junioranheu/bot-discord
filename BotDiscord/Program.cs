using BotDiscord.Common;
using BotDiscord.Init;
using BotDiscord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder().
             AddJsonFile($"appsettings.json").
             AddEnvironmentVariables().
             Build();

var commands = new CommandService(new CommandServiceConfig
{
    // Again, log level:
    LogLevel = LogSeverity.Info,

    // There's a few more properties you can set,
    // for example, case-insensitive commands.
    CaseSensitiveCommands = false
});

var clientConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent
};

var client = new DiscordSocketClient(clientConfig);

// Setup your DI container.
Bootstrapper.Init();
await Bootstrapper.RegisterInstance(client, "client");
await Bootstrapper.RegisterInstance(commands, "commands");
await Bootstrapper.RegisterInstance(config, "config");
Bootstrapper.RegisterType<ICommandHandler, CommandHandler>();

await MainAsync();

async Task MainAsync()
{
    await Bootstrapper.ServiceProvider!.GetRequiredService<ICommandHandler>().InitializeAsync();

    client.Ready += async () =>
    {
        await Logger.Log(LogSeverity.Info, "Ready", $"Number {Guid.NewGuid} is connected and ready!");
    };

    // Login and connect.
    var token = config.GetRequiredSection("Settings")["DiscordBotToken"];
    if (string.IsNullOrWhiteSpace(token))
    {
        await Logger.Log(LogSeverity.Error, $"{nameof(Program)} | {nameof(MainAsync)}", "Token is null or empty.");
        return;
    }

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    // Wait infinitely so your bot actually stays connected.
    await Task.Delay(Timeout.Infinite);
}