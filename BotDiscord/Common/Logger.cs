using Discord;

namespace BotDiscord.Common
{
    public sealed class Logger
    {
        public static async Task Log(LogSeverity severidade, string fonte, string mensagem, Exception? excecao = null)
        {
            await Log(new LogMessage(severidade, fonte, mensagem, excecao));
        }

        private static Task Log(LogMessage log)
        {
            switch (log.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"{Utils.HorarioBrasilia()} [{log.Severity}] {log.Source}: {log.Message} {log.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}