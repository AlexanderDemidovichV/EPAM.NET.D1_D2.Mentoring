using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using EPAM.Mentoring.Exceptions;
using EPAM.Mentoring.Models;
using EPAM.Mentoring._04.Resources;
using LogAdapter;
using LogProvider;
using WatcherChangeTypes = System.IO.WatcherChangeTypes;

namespace EPAM.Mentoring
{
    public class Watcher
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

            InitWatchers(directoryPathsToListen);
        }

        public void OnFileSystemEvent(object source, FileSystemEventArgs e)
        {
            logger.Info(Messages.Found + Messages.FileName + e.Name);

            try
            {
                if (!CheckRules(e.FullPath))
                    UseDefaultRule(e.FullPath);
            }
            catch (IOException ex)
            {
                logger.Fatal(Messages.ErrorHasOccurred + ex.Message +
                     Environment.NewLine + ex.StackTrace);
                throw new FileSystemWatcherException(Messages.ErrorHasOccurred, ex);
            }
            catch (ArgumentException ex)
            {
                logger.Fatal(Messages.ErrorHasOccurred + ex.Message +
                    Environment.NewLine + ex.StackTrace);
                throw new FileSystemWatcherException(Messages.ErrorHasOccurred, ex);
            }

            logger.Info(Messages.FileName + e.Name + Messages.FullPath + 
                e.FullPath + Messages.Delimiter + 
                GetChangeTypeMessage(e.ChangeType));
        }

        public void OnError(object source, ErrorEventArgs e)
        {
            logger.Error(Messages.ErrorHasOccurred + e.GetException().Message + 
                Environment.NewLine + e.GetException().StackTrace);
        }

        private void InitWatchers(IEnumerable<string> directoryPathsToListen)
        {
            foreach (var directoryPath in directoryPathsToListen) {
                var watcher = new FileSystemWatcher {
                    Path = Path.GetFullPath(directoryPath),
                    NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName
                };
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
                        Path.GetFileNameWithoutExtension(filePath) + " " +
                        ++rule.Counter + Path.GetExtension(filePath);
                    break;
                case ActionToTakeWhenInputFileNameIsChanged.AddMoveDate:
                    newFilePath = rule.DestinationFolder + 
                        "\\" + Path.GetFileNameWithoutExtension(filePath) + " " +
                        DateTimeOffset.Now.ToString("yy-MM-dd-hh,mm,ss") + 
                        Path.GetExtension(filePath);
                    break;
            }
            File.Move(filePath, newFilePath);

            logger.Info(Messages.FileName + $"{newFilePath}" +
                string.Format(Messages.MoveTo, newFilePath));
        }

        private string GetChangeTypeMessage(WatcherChangeTypes changeType)
        {
            switch (changeType) {
                case WatcherChangeTypes.Created:
                    return WatcherChangeTypeMessages.Created;
                case WatcherChangeTypes.Renamed:
                    return WatcherChangeTypeMessages.Renamed;
                default:
                    return WatcherChangeTypeMessages.NotSupported;
            }
        }
    }
}
