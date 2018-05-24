using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LogAdapter;

namespace InternetDownloader
{
    public class FileSaver
    {
        private const int bufferSize = 2000;
        private readonly string rootPath;
        private readonly string folder;
        private readonly string url;
        private readonly string protocol;
        private readonly ILogger logger;

        public FileSaver(string url, string rootPath, string folder, string protocol, ILogger logger)
        {
            this.url = url;
            this.rootPath = rootPath;
            this.folder = folder;
            this.protocol = protocol;
            this.logger = logger;
        }

        public void SaveSource()
        {
            SaveSourceAsync(url);
        }

        private async void SaveSourceAsync(string sourceUrl)
        {
            logger.Info("Resource founded");
            var fileName = Path.GetFileName(sourceUrl);

            fileName = "_" + fileName;
            var fullName = Path.Combine(folder, fileName);

            if (!sourceUrl.StartsWith("http"))
                if (sourceUrl.StartsWith("//")) {
                    sourceUrl = protocol + sourceUrl;
                } else {
                    sourceUrl = rootPath + sourceUrl.Substring(1);
                }

            try {
                await DownloadAsync(sourceUrl, fullName);
            } catch (Exception ex) {
                logger.Error("Error while downloading");
            }
        }

        private async Task DownloadAsync(string requestUri, string filename)
        {
            if (requestUri == null)
                throw new InternetDownloaderException(nameof(requestUri));
 
            await DownloadAsync(new Uri(requestUri), filename);
        }
 
        private async Task DownloadAsync(Uri requestUri, string filename)
        {
            if (filename == null)
                throw new InternetDownloaderException(nameof(filename));

            using (var httpClient = new HttpClient()) {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri)) {
                    using (
                        Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
                            stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true)) {
                        await contentStream.CopyToAsync(stream);
                    }
                }
            }
        }
    }
}
