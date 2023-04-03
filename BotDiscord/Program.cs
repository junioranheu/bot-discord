using BotDiscord;
using BotDiscord.Common;
using BotDiscord.Init;
using BotDiscord.Models;
using BotDiscord.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder().
             AddJsonFile("appsettings.json").
             AddEnvironmentVariables().
             AddUserSecrets<Program>().
             Build();

ConfigurationBuilderInjectionResponse configInjection = ConfigurationBuilderInjection.AddConfigurationBuilder(config);

if (string.IsNullOrEmpty(configInjection.Token) | configInjection.Client is null)
{
    await Logger.Log(LogSeverity.Info, "XXXXXXXXX", $"Token ou client estão vazios");
    return;
}

await Main(configInjection.Token!, configInjection.Client!);

static async Task Main(string token, DiscordSocketClient client)
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