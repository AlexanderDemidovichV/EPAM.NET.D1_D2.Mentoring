using System;

namespace InternetDownloader
{
    [Serializable]
    public class InternetDownloaderException : Exception
    {
        public InternetDownloaderException()
        {
        }

        public InternetDownloaderException(string message) : base(message)
        {
        }

        public InternetDownloaderException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}
