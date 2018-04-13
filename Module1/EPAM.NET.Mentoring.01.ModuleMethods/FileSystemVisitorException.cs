using System;

namespace EPAM.NET.Mentoring
{
    [Serializable]
    public class FileSystemVisitorException : Exception
    {
        public FileSystemVisitorException()
        {
        }

        public FileSystemVisitorException(string message) 
            : base(message)
        {
        }

        public FileSystemVisitorException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
