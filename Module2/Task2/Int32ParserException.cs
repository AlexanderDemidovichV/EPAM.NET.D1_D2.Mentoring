using System;

namespace Task2
{
    [Serializable]
    public class Int32ParserException: Exception
    {
        public Int32ParserException()
        {
        }

        public Int32ParserException(string message) : base(message)
        {
        }

        public Int32ParserException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
