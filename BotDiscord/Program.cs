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

ConfigurationBuilderInjectionResponse configInjection = await ConfigurationBuilderInjection.AddConfigurationBuilder(config);

if (string.IsNullOrEmpty(configInjection.Token) | configInjection.Client is null)
{
    await Logger.Log(LogSeverity.Error, $"{nameof(Program)}", $"Token ou _client estão vazios ou inválidos");
    return;
}

await Main(configInjection.Token!, configInjection.Client!);

static async Task Main(string token, DiscordSocketClient client)
{
    await Bootstrapper.ServiceProvider!.GetRequiredService<ICommandHandlerService>().Initialize();

    client.Ready += async () =>
    {
        await Logger.Log(LogSeverity.Info, $"{nameof(Program)}", $"Bot está conectado e pronto para uso — {Guid.NewGuid()}");
    };

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    await Task.Delay(Timeout.Infinite);
}