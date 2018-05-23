using System;
using NLog;

namespace LogAdapter
{
    public class NLoggerAdapter : ILogger
    {
        private readonly Logger logger;

        public NLoggerAdapter(Logger logger)
        {
            if (ReferenceEquals(logger, null))
                throw new ArgumentNullException($"{nameof(logger)} is null.");

            this.logger = logger;
        }

        public void Info(string message) => logger.Info(message);

        public void Info(Exception exception, string message, params object[] args) => logger.Info(exception, message, args);

        public void Info(string message, params object[] args) => logger.Info(message, args);

        public void Error(string message) => logger.Error(message);

        public void Error(Exception exception, string message, params object[] args) => logger.Error(exception, message, args);

        public void Error(string message, params object[] args) => logger.Error(message, args);

        public void Fatal(string message) => logger.Fatal(message);

        public void Fatal(Exception exception, string message, params object[] args) => logger.Fatal(exception, message, args);

        public void Fatal(string message, params object[] args) => logger.Fatal(message, args);
    }
}