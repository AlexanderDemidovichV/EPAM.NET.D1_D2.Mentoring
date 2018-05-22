using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LogAdapter;
using LogProvider;

namespace InternetDownloader
{
    public class InternetDownloader
    {
        private readonly ILogger logger = NLogProvider.GetLogger("NLog",
            CultureInfo.CurrentCulture);

        private readonly string startUrl;
        private readonly string storagePath;
        private readonly int nestLevel;
        private readonly InternetDownloaderOption option;
        private readonly string[] extensionConstraints;

        public InternetDownloader(string startUrl, string storagePath,
            int nestLevel, 
            InternetDownloaderOption option, 
            string[] extensionConstraints)
        {
            this.startUrl = startUrl;
            this.storagePath = storagePath;
            this.nestLevel = nestLevel;
            this.option = option;
            this.extensionConstraints = extensionConstraints;
        }

        public async Task DownloadContent()
        {
            var document = new HtmlDocument();
            try {
                using (HttpClient client = new HttpClient()) {
                    var response =
                        await client.GetAsync("http://httpbin.org/get");

                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync()) {
                        document.Load(stream, Encoding.UTF8);
                    }
                }
            }
            catch (Exception e) {
                throw;
            }
            
        }
    }
}
