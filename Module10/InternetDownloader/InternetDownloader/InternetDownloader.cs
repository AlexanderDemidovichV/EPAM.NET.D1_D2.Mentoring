using System;
using System.IO;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using LogAdapter;

namespace InternetDownloader
{
    public class InternetDownloader
    {
        private readonly ILogger logger;

        private const int pathLimit = 70;
        private readonly string startUrl;
        private readonly string storagePath;
        private readonly int nestLevelLimit;
        private readonly InternetDownloaderOption option;
        private readonly string[] extensionConstraints;

        public InternetDownloader(ILogger logger, string startUrl, string storagePath,
            int nestLevelLimit, 
            InternetDownloaderOption option, 
            string[] extensionConstraints)
        {
            this.logger = logger;
            this.startUrl = startUrl;
            this.storagePath = storagePath;
            this.nestLevelLimit = nestLevelLimit;
            this.option = option;
            this.extensionConstraints = extensionConstraints;
        }

        public void DownloadContent()
        {
            DownloadContent(startUrl, 0, storagePath);
        }

        public void DownloadContent(string startUrl, int level, string folder)
        {
            if (level > nestLevelLimit) {
                return;
            }

            var url = new Uri(startUrl);
            var document = LoadHtmlDocument(url);

            string pathString = Path.Combine(folder, GetDirectoryName(startUrl));
            if (!Directory.Exists(pathString)) {
               Directory.CreateDirectory(pathString);
            }

            var linkService = new LinkService(document.DocumentNode, pathString, url.AbsoluteUri, 
                extensionConstraints, level, nestLevelLimit, logger, option);
            var linkList = linkService.Search();

            document.Save(Path.Combine(pathString, "index.html"));

            foreach (var link in linkList) {
                logger.Info(link);
                DownloadContent(link, level + 1, pathString);
            }
        }

        public static string GetDirectoryName(string path)
        {
            if (path == null)
                throw new InternetDownloaderException($"Argument null: {nameof(path)}");

            try {
                var uri = new Uri(path);
                var url = uri.OriginalString.Replace(".", "").Replace(@"/", "").Replace(":", "")
                    .Replace("?", "").Replace("|","") ;

                return url.Substring(url.Length - pathLimit > 0 ? url.Length - pathLimit : 0);
            } catch (UriFormatException) {
                throw new InternetDownloaderException("Uri format problems");
            }
        }

        private HtmlDocument LoadHtmlDocument(Uri url)
        {
            var document = new HtmlDocument();
            

            using (HttpClient client = new HttpClient()) {
                var response =
                    client.GetAsync(url.AbsoluteUri).Result;

                try {
                    response.EnsureSuccessStatusCode();
                } catch (HttpRequestException e){
                    logger.Error(e.Message);
                }

                using (var stream = response.Content.ReadAsStreamAsync().Result) {
                    document.Load(stream, Encoding.UTF8);
                }

                document.Save(Path.Combine(storagePath, "index.html"));
            }
            return document;
        }
    }
}
