using System;
using log4net;
using log4net.Appender;
using log4net.Repository;
using LogAdapter;
using ILogger = LogAdapter.ILogger;

namespace LogProvider
{
    public static class Log4NetProvider
    {
        public static ILogger LogProvider(string className)
        {
            if (className == null)
                throw new ArgumentNullException($"{nameof(className)} is null.");

            var logger = LogManager.GetLogger(className);
            return new Log4NetAdapter(logger);
        }

        public static void Flush(string className)
        {
            ILoggerRepository rep = LogManager.GetRepository();
            foreach (IAppender appender in rep.GetAppenders()) {
                var buffered = appender as BufferingAppenderSkeleton;
                buffered?.Flush();
            }
        }
    }
}