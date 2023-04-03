using Discord.WebSocket;

namespace BotDiscord.Models
{
    public sealed class ConfigurationBuilderInjectionResponse
    {
        public string? Token { get; set; } = string.Empty;

        public DiscordSocketClient? Client { get; set; } = null;
    }
}