using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Redbox.NetCore.Logging.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogDebugWithSource(this ILogger logger, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogDebug(string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogInfoWithSource(this ILogger logger, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogInformation(string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogErrorWithSource(this ILogger logger, Exception ex, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogError(ex, string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogErrorWithSource(this ILogger logger, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogError(string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogWarningWithSource(this ILogger logger, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogWarning(string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogCriticalWithSource(this ILogger logger, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogCritical(string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        public static void LogCriticalWithSource(this ILogger logger, Exception ex, string message,
            [CallerMemberName] string memberName = "", [CallerFilePath] string callerLocation = "")
        {
            logger.LogCritical(ex, string.Concat(callerLocation.ClassName(), ".", memberName, " -> ", message));
        }

        private static string ClassName(this string callerLocation)
        {
            if (!callerLocation.Contains("\\"))
            {
                var text = callerLocation.Split('/').LastOrDefault();
                if (text == null) return null;
                return text.TrimEnd(".cs");
            }

            var text2 = callerLocation.Split('\\').LastOrDefault();
            if (text2 == null) return null;
            return text2.TrimEnd(".cs");
        }

        private static string TrimEnd(this string value, string substring)
        {
            if (value == null || !value.EndsWith(substring)) return value;
            return value.Remove(value.LastIndexOf(substring));
        }
    }
}