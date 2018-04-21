using System;
using System.ComponentModel;

namespace LogAdapter
{
    public interface ILogger
    {
        void Info([Localizable(false)] string message);

        void Info(Exception exception, [Localizable(false)] string message, params object[] args);

        void Info([Localizable(false)] string message, params object[] args);

        void Error([Localizable(false)] string message);

        void Error(Exception exception, [Localizable(false)] string message, params object[] args);

        void Error([Localizable(false)] string message, params object[] args);

        void Fatal([Localizable(false)] string message);

        void Fatal(Exception exception, [Localizable(false)] string message, params object[] args);

        void Fatal([Localizable(false)] string message, params object[] args);
    }
}
