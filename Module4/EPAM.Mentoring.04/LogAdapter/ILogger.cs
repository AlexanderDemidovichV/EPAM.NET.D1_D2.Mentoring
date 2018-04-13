using System;
using System.ComponentModel;

namespace LogAdapter
{
    public interface ILogger
    {
        void Trace([Localizable(false)] string message);

        void Trace(Exception exception, [Localizable(false)] string message, params object[] args);

        void Trace([Localizable(false)] string message, params object[] args);

        void Debug([Localizable(false)] string message);

        void Debug(Exception exception, [Localizable(false)] string message, params object[] args);

        void Debug([Localizable(false)] string message, params object[] args);

        void Info([Localizable(false)] string message);

        void Info(Exception exception, [Localizable(false)] string message, params object[] args);

        void Info([Localizable(false)] string message, params object[] args);

        void Warn([Localizable(false)] string message);

        void Warn(Exception exception, [Localizable(false)] string message, params object[] args);

        void Warn([Localizable(false)] string message, params object[] args);

        void Error([Localizable(false)] string message);

        void Error(Exception exception, [Localizable(false)] string message, params object[] args);

        void Error([Localizable(false)] string message, params object[] args);

        void Fatal([Localizable(false)] string message);

        void Fatal(Exception exception, [Localizable(false)] string message, params object[] args);

        void Fatal([Localizable(false)] string message, params object[] args);
    }
}
