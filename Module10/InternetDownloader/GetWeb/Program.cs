using System;
using InternetDownloader;

namespace GetWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var down = new InternetDownloader.InternetDownloader(
                "http://www.google.com",
                @"d:\",
                1,
                InternetDownloaderOption.WithoutConstraints,
                new string[0]);

            down.DownloadContent();

            Console.ReadKey();
        }
    }
}
