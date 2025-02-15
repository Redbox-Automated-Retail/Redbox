using System;
using Redbox.HAL.Component.Model;

namespace HALUtilities
{
    internal sealed class ConsoleLogger : ILogger
    {
        private readonly bool EnableDebug;

        internal ConsoleLogger(bool enableDebug)
        {
            EnableDebug = enableDebug;
            ServiceLocator.Instance.AddService<ILogger>(this);
        }

        public void Log(string message, Exception e)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, LogEntryType type)
        {
            Console.WriteLine(message);
        }

        public void Log(string message, Exception e, LogEntryType type)
        {
            Console.WriteLine(message);
        }

        public bool IsLevelEnabled(LogEntryType entryLevel)
        {
            return LogEntryType.Debug != entryLevel || EnableDebug;
        }
    }
}