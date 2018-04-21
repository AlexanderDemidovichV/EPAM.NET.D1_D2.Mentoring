using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using EPAM.Mentoring.Interfaces;
using EPAM.Mentoring.Models;
using EPAM.Mentoring._04.Resources;
using LogAdapter;
using LogProvider;

namespace EPAM.Mentoring
{
    public class Watcher: IWatcher
    {
        private static readonly FileSystemWatcherSettings settings =
            (FileSystemWatcherSettings)ConfigurationManager.
            GetSection("FileSystemWatcherSettings");

        private readonly ILogger logger = NLogProvider.GetLogger("NLog",
            settings.CultureInfo);

        private List<RuleModel> rules;

        public void StartListen()
        {
            Thread.CurrentThread.CurrentUICulture = settings.CultureInfo;

            logger.Info(Messages.StartApplication);

            var directoryPathsToListen = from Directory directory in settings.Directories
                select directory.Path;

            rules = (from Rule rule in settings.Rules
                select new RuleModel {
                    FilePattern = rule.FilePattern,
                    DestinationFolder = rule.DestinationFolder,
                    ActionToTakeWhenInputFileNameIsChanged = 
                        rule.ActionToTakeWhenInputFileNameIsChanged,
                    Counter = 0
                }).ToList();

            //ValidateSettings(directoryPathsToListen);

            InitWatchers(directoryPathsToListen);
        }

        public void OnFileSystemEvent(object source, FileSystemEventArgs e)
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
                    Path = Path.GetFullPath(directoryPath),
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };
                watcher.Changed += OnFileSystemEvent;
                watcher.Created += OnFileSystemEvent;
                watcher.Renamed += OnFileSystemEvent;
                watcher.Error += OnError;
                watcher.EnableRaisingEvents = true;
            }
        }

        private bool CheckRules(string filePath)
        {
            foreach (var rule in rules) {
                if (Regex.IsMatch(Path.GetFileName(filePath), rule.FilePattern)){
                    ModifyAndMoveFile(filePath, rule);
                    return true;
                }
            }
            return false;
        }

        private void UseDefaultRule(string filePath)
        {
            ModifyAndMoveFile(filePath, new RuleModel {
                FilePattern = ".*?",
                DestinationFolder = "c:\\default",
                ActionToTakeWhenInputFileNameIsChanged =
                    ActionToTakeWhenInputFileNameIsChanged.AddMoveDate
            });
        }

        private void ModifyAndMoveFile(string filePath, RuleModel rule)
        {
            string newFilePath = filePath;

            switch (rule.ActionToTakeWhenInputFileNameIsChanged) {
                case ActionToTakeWhenInputFileNameIsChanged.AddIndexNumber:
                    newFilePath = rule.DestinationFolder + "\\" + 
                        Path.GetFileNameWithoutExtension(filePath) + 
                        ++rule.Counter + Path.GetExtension(filePath);
                    break;
                case ActionToTakeWhenInputFileNameIsChanged.AddMoveDate:
                    newFilePath = Path.GetDirectoryName(rule.DestinationFolder) + 
                        "\\" + Path.GetFileNameWithoutExtension(filePath) + 
                        DateTimeOffset.Now + Path.GetExtension(filePath);
                    break;
            }
            File.Move(filePath, newFilePath);

            logger.Info(Messages.FileName + $"{newFilePath}" +
                string.Format(Messages.MoveTo, rule.DestinationFolder + newFilePath));
        }
    }
}
