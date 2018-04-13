using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using EPAM.Mentoring.Interfaces;
using LogAdapter;
using LogProvider;

namespace EPAM.Mentoring
{
    public class Watcher: IWatcher
    {
        private static readonly FileSystemWatcherSettings settings =
            (FileSystemWatcherSettings)ConfigurationManager.GetSection("FileSystemWatcherSettings");

        private readonly ILogger logger = NLogProvider.GetLogger("Pro",
            settings.CultureInfo);

        public void StartListen()
        {
            logger.Info("Start application:");

            Thread.CurrentThread.CurrentUICulture = settings.CultureInfo;

            var directoryPathsToListen = from Directory directory in settings.Directories
                select directory.Path;

            var rules = (from Rule rule in settings.Rules
                select new {
                    rule.FilePattern,
                    rule.DestinationFolder,
                    rule.ActionToTakeWhenInputFileNameIsChanged
                }).ToList();

            InitWatchers(directoryPathsToListen);
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        public void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        public void OnDelete(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        public void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("Name: {0} " + Environment.NewLine + "renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public void OnError(object source, ErrorEventArgs e)
        {
            Console.WriteLine("Error was occured: " + e.GetException().Message + Environment.NewLine +
                e.GetException().StackTrace + Environment.NewLine +
                e.GetException().InnerException.Message + Environment.NewLine +
                e.GetException().InnerException.StackTrace);
        }

        private void InitWatchers(IEnumerable<string> directoryPathsToListen)
        {
            foreach (var directoryPath in directoryPathsToListen) {
                var watcher = new FileSystemWatcher {
                    Path = directoryPath,
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*.*"
                };
                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDelete;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;
                watcher.EnableRaisingEvents = true;
            }
        }
    }
}
