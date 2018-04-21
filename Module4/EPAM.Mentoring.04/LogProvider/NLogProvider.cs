using System;
using System.Globalization;
using LogAdapter;
using NLog;
using ILogger = LogAdapter.ILogger;

namespace LogProvider
{
    public static class NLogProvider
    {
        public static ILogger GetLogger(string className, CultureInfo defaultCultureInfo)
        {
            if (ReferenceEquals(className, null))
                throw new ArgumentNullException($"{nameof(className)} is null.");

            var config = new NLog.Config.LoggingConfiguration();

            var logFile = new NLog.Targets.FileTarget() { FileName = "log.txt", Name = "logfile" };
            var logConsole = new NLog.Targets.ConsoleTarget() { Name = "logconsole" };

            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logConsole));
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logFile));
            config.DefaultCultureInfo = defaultCultureInfo;

            LogManager.Configuration = config;

            var logger = LogManager.GetLogger(className);
            return new NLoggerAdapter(logger);
        }

        public static void Flush() => LogManager.Flush();
    }
}
