using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private dynamic rules;

        public void StartListen()
        {
            logger.Info("Start application:");

            Thread.CurrentThread.CurrentUICulture = settings.CultureInfo;

            var directoryPathsToListen = from Directory directory in settings.Directories
                select directory.Path;

            rules = (from Rule rule in settings.Rules
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

        private void CheckFilePattern(string fileName, string filePath)
        {
            if (rules == null) {
                throw new ArgumentNullException(nameof(rules));
            }
            
            foreach (Rule rule in rules)
            {
                if (Regex.IsMatch(filePath, rule.FilePattern))
                    ModifyAndMoveFile(fileName, filePath, rule);
            }
        }

        private void ModifyAndMoveFile(string fileName, string filePath, Rule rule)
        {
            string newFilePath = string.Empty;

            switch (rule.ActionToTakeWhenInputFileNameIsChanged) {
                case ActionToTakeWhenInputFileNameIsChanged.AddIndexNumber:
                    newFilePath = filePath + "woof";
                    File.Move(filePath, newFilePath);
                    break;
                case ActionToTakeWhenInputFileNameIsChanged.AddMoveDate:
                    newFilePath = filePath + DateTimeOffset.Now;
                    File.Move(filePath, newFilePath);
                    break;
            }

            File.Move(newFilePath, rule.DestinationFolder + fileName);

            //from = System.IO.Path.Combine(@"E:\vid\", "(" + i.ToString() + ").PNG");
            //to = System.IO.Path.Combine(@"E:\ConvertedFiles\", i.ToString().PadLeft(6, '0') + ".png");

            //File.Move(from, to); // Try to move
        }
    }
}
