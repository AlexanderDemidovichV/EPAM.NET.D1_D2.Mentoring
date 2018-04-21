using System;

namespace EPAM.Mentoring.Exceptions
{
    public class FileSystemWatcherException: Exception
    {
        public FileSystemWatcherException()
        {
        }

        public FileSystemWatcherException(string message) : base(message)
        {
        }

        public FileSystemWatcherException(string message, Exception inner) : 
            base(message, inner)
        {
        }
    }
}
