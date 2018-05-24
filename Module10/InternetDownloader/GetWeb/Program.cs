using System;
using System.Globalization;
using InternetDownloader;
using LogAdapter;
using LogProvider;

namespace GetWeb
{
    class Program
    {
        private static readonly ILogger logger = NLogProvider.GetLogger("NLog",
            CultureInfo.CurrentCulture);

        static void Main(string[] args)
        {
            
            var down = new InternetDownloader.InternetDownloader(
                logger,
                "http://www.google.com",
                @"d:\",
                1,
                InternetDownloaderOption.WithoutConstraints,
                new string[0]);

            try {
                down.DownloadContent();
            } catch (InternetDownloaderException e) {
                logger.Error(e.Message);
            }

            Console.ReadKey();
        }
    }
}
