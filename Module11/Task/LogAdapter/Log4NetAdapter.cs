using System;
using log4net;

namespace LogAdapter
{
    public class Log4NetAdapter : ILogger
    {
        private readonly ILog logger;

        public Log4NetAdapter(ILog logger)
        {
            this.logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} is null.");
        }

        public void Debug(string message) => logger.Debug(message);

        public void Debug(Exception exception, string message) => logger.Debug(message, exception);
        
        public void Info(string message) => logger.Info(message);

        public void Info(Exception exception, string message) => logger.Info(message, exception);

        public void Info(string message, params object[] args) => logger.InfoFormat(message, args);

        public void Error(string message) => logger.Error(message);

        public void Error(Exception exception, string message) => logger.Error(message, exception);

        public void Error(string message, params object[] args) => logger.ErrorFormat(message, args);

        public void Fatal(string message) => logger.Fatal(message);

        public void Fatal(Exception exception, string message) => logger.Fatal(message, exception);

        public void Fatal(string message, params object[] args) => logger.FatalFormat(message, args);
    }
}