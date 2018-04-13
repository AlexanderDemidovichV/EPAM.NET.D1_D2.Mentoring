using System;

namespace Task1
{
    [Serializable]
    public class FormatApplicationException : Exception
    {
        public FormatApplicationException()
        {
        }

        public FormatApplicationException(string message) : base(message)
        {
        }

        public FormatApplicationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
