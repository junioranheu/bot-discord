using BotDiscord.Common;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace BotDiscord.Init
{
    public static class Bootstrapper
    {
        public static IServiceProvider? ServiceProvider { get; set; }
        private static IServiceCollection? _serviceCollection;
        private static bool _isInitialized = false;

        public static void Init()
        {
            if (!_isInitialized)
            {
                var serviceCollection = new ServiceCollection();
                var serviceProvider = serviceCollection.BuildServiceProvider();

                _serviceCollection = serviceCollection;
                ServiceProvider = serviceProvider;
                _isInitialized = true;
            }
        }

        public static void RegisterType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _serviceCollection?.AddSingleton<TInterface, TImplementation>();
            ServiceProvider = _serviceCollection?.BuildServiceProvider();
        }

        public async static Task RegisterInstance<TInterface>(TInterface instance, string? nome) where TInterface : class
        {
            _serviceCollection?.AddSingleton<TInterface>(instance);
            ServiceProvider = _serviceCollection?.BuildServiceProvider();
            await Logger.Log(LogSeverity.Info, $"{nameof(Bootstrapper)}", $"RegisterInstance {nome} instanced.");
        }
    }
}