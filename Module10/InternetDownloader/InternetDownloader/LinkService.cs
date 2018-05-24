using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using LogAdapter;

namespace InternetDownloader
{
    public class LinkService
    {
        private readonly HtmlNode htmlNode;
        private readonly string rootFolder;
        private readonly string rootPath;
        private readonly string protocol;
        private readonly int level;
        private readonly int nestLevelLimit;
        private readonly string[] extensionConstraints;
        private readonly ILogger logger;
        private InternetDownloaderOption option;
        private List<string> _links;

        public LinkService(HtmlNode htmlNode, string rootFolder, string rootPath, 
            string[] extensionConstraints, int level, int nestLevelLimit, ILogger logger, InternetDownloaderOption option)
        {
            this.option = option;
            this.logger = logger ?? throw new InternetDownloaderException($"Argument is null: {nameof(logger)}");
            this.htmlNode = htmlNode;
            this.rootFolder = rootFolder;
            this.rootPath = RemoveParamsFromUrl(rootPath);
            protocol = rootPath.Substring(0, rootPath.IndexOf('/'));
            this.extensionConstraints = extensionConstraints;
            this.level = level;
            this.nestLevelLimit = nestLevelLimit;
            _links = new List<string>(0);
        }

        public List<string> Search()
        {
            Search(htmlNode);
            return _links;
        }

        private void Search(HtmlNode parent)
        {
            foreach (var child in parent.ChildNodes) {
                GetRefs(child.Attributes);
                GetFiles(child.Attributes);
                Search(child);
            }
        }

        public string RemoveParamsFromUrl(string url)
        {
            if (url == null)
                throw new InternetDownloaderException($"Argument null: {nameof(url)}");

            return url.Split('?')[0];
        }

        private void GetRefs(HtmlAttributeCollection htmlAttributes)
        {
            var hrefAttr = htmlAttributes["href"];
            if (hrefAttr != null) {
                var link = hrefAttr.Value;

                var path = RemoveParamsFromUrl(link);
                var extension = Path.GetExtension(path);
                
                if (extensionConstraints.Contains(extension)) {
                    var fileSaver = new FileSaver(path, rootPath, rootFolder, protocol, logger);
                    fileSaver.SaveSource();
                    return;
                }

                logger.Info("Link founded " + link);
                if (!link.StartsWith("http")) {
                    if (link.StartsWith("//")) {
                        link = protocol + link;
                    } else {
                        link = rootPath + link;
                    }
                }

                var newLink = InternetDownloader.GetDirectoryName(link);
                if (level + 1 <= nestLevelLimit) {
                    if (option == InternetDownloaderOption.WithinDomain && new Uri(link).Host != new Uri(rootPath).Host)
                        return;
                    hrefAttr.Value = Path.Combine(newLink, "index.html");
                    _links.Add(link);
                }
            }
        }

        private void GetFiles(HtmlAttributeCollection htmlAttributes)
        {
            var srcAttr = htmlAttributes["src"];
            if (srcAttr == null) return;

            var link = srcAttr.Value;
            var urlWithoutParams = RemoveParamsFromUrl(link);

            if (String.IsNullOrEmpty(urlWithoutParams)) return;

            if (extensionConstraints.Length == 0 || 
                extensionConstraints.Contains(Path.GetExtension(urlWithoutParams))){
                var fileSaver = new FileSaver(urlWithoutParams, rootPath, rootFolder, protocol, logger);
                fileSaver.SaveSource();
            }
        }
    }
}
