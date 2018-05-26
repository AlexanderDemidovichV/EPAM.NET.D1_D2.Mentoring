using System;

namespace LogAdapter
{
    public interface ILogger
    {
        void Debug(string message);

        void Debug(Exception exception, string message);

        void Info(string message);

        void Info(Exception exception, string message);

        void Info(string message, params object[] args);

        void Error(string message);

        void Error(Exception exception, string message);

        void Fatal(string message);

        void Fatal(Exception exception, string message);
    }
}