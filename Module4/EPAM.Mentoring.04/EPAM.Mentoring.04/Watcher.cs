using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using EPAM.Mentoring.Interfaces;
using EPAM.Mentoring._04.Resources;
using LogAdapter;
using LogProvider;

namespace EPAM.Mentoring
{
    public class Watcher: IWatcher
    {
        private static readonly FileSystemWatcherSettings settings =
            (FileSystemWatcherSettings)ConfigurationManager.GetSection("FileSystemWatcherSettings");

        private readonly ILogger logger = NLogProvider.GetLogger("NLog",
            settings.CultureInfo);

        private dynamic rules;

        public void StartListen()
        {
            logger.Info(Messages.StartApplication);

            Thread.CurrentThread.CurrentUICulture = settings.CultureInfo;

            var directoryPathsToListen = from Directory directory in settings.Directories
                select directory.Path;

            rules = (from Rule rule in settings.Rules
                select new {
                    rule.FilePattern,
                    rule.DestinationFolder,
                    rule.ActionToTakeWhenInputFileNameIsChanged,
                    Counter = 0
                }).ToList();

            if (rules == null || directoryPathsToListen == null) {
                throw new ArgumentNullException(nameof(rules));
            }

            InitWatchers(directoryPathsToListen);
        }

        public void OnChanged(object source, FileSystemEventArgs e)
        {
            FindRule(e);
        }

        public void OnCreated(object source, FileSystemEventArgs e)
        {
            FindRule(e);
        }

        public void OnDelete(object source, FileSystemEventArgs e)
        {
            FindRule(e);
        }

        public void OnRenamed(object source, RenamedEventArgs e)
        {
            FindRule(e);
        }

        private void FindRule(FileSystemEventArgs e)
        {
            logger.Info(Messages.Found + Messages.FileName + e.Name);

            if (!CheckRules(e.FullPath))
                UseDefaultRule(e.FullPath);

            logger.Info(Messages.FileName + Environment.NewLine + e.Name +
                        Messages.FullPath + e.FullPath + Messages.Delimiter + e.ChangeType);
        }

        public void OnError(object source, ErrorEventArgs e)
        {
            logger.Error(Messages.ErrorHasOccurred + e.GetException().Message + Environment.NewLine +
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
                    Filter = "*"
                };
                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDelete;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;
                watcher.EnableRaisingEvents = true;
            }
        }

        private bool CheckRules(string filePath)
        {
            foreach (var rule in rules) {
                if (Regex.IsMatch(filePath, rule.FilePattern)){
                    ModifyAndMoveFile(filePath, rule);
                    return true;
                }
            }
            return false;
        }

        private void UseDefaultRule(string filePath)
        {
            ModifyAndMoveFile(filePath, new {
                FilePattern = "*",
                DestinationFolder = "",
                ActionToTakeWhenInputFileNameIsChanged = ActionToTakeWhenInputFileNameIsChanged.AddMoveDate
            });
        }

        private void ModifyAndMoveFile(string filePath, dynamic rule)
        {
            string newFilePath = filePath;

            switch (rule.ActionToTakeWhenInputFileNameIsChanged) {
                case ActionToTakeWhenInputFileNameIsChanged.AddIndexNumber:
                    newFilePath = filePath + rule.Counter++;
                    File.Move(filePath, newFilePath);
                    break;
                case ActionToTakeWhenInputFileNameIsChanged.AddMoveDate:
                    newFilePath = filePath + DateTimeOffset.Now;
                    File.Move(filePath, newFilePath);
                    break;
            }

            File.Move(newFilePath, rule.DestinationFolder + newFilePath);

            logger.Info(Messages.FileName + $"{newFilePath}" +
                        string.Format(Messages.MoveTo, rule.DestinationFolder + newFilePath));
        }
    }
}
