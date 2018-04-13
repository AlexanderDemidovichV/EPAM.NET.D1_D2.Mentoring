using System;

namespace EPAM.NET.Mentoring
{
    public class FileSystemEventArgs : EventArgs
    {
        public string Message { get; }

        public FileSystemEventArgs(string message)
        {
            Message = message;
        }
    }
}
