using TimeZoneConverter;

namespace BotDiscord.Common
{
    public static class Utils
    {
        public static DateTime HorarioBrasilia()
        {
            TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
        }
    }
}