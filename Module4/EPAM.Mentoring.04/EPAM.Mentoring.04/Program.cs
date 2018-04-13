using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using EPAM.Mentoring._04;
using EPAM.Mentoring._04.Resources;
using LogAdapter;
using LogProvider;

namespace EPAM.Mentoring
{


    class Program
    {
        public static readonly ILogger logger = NLogProvider.GetLogger("Pro");

        static void Main(string[] args)
        {
            logger.Info("dsfsdfdsfsd");

            var configvalue1 = ConfigurationManager.AppSettings;
            var tguhj = Resource1.String1;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            var tgusadasdahj = Resource1.String1;


            var mySection = (FileSystemWatcherSettings)ConfigurationManager.GetSection("FileSystemWatcherSettings");
            //var tabs = mySection.Rules;
            var tw = mySection.Directories;
            foreach (var tab in tw) {
                var t = tab;
            }
            var tabs = mySection.Rules;

            AttributeCollection attributes = TypeDescriptor.GetProperties(typeof(ILogger))["Trace"].Attributes;

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "c:\\Users\\Aliaksandr_Dzemidovi\\source\\repos\\EPAM.Mentoring.04\\EPAM.Mentoring.04\\bin\\";

            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite
                                  | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            //watcher.Filter = "*.txt";

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDelete;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        private static void OnDelete(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Name: " + Environment.NewLine + e.Name + " Full Path: " + e.FullPath + " | " + e.ChangeType);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("Name: {0} " + Environment.NewLine + "renamed to {1}", e.OldFullPath, e.FullPath);
        }

        private static void OnError(object source, ErrorEventArgs e)
        {
            Console.WriteLine("Error was occured: " + e.GetException().Message + Environment.NewLine + 
                e.GetException().StackTrace + Environment.NewLine +
                e.GetException().InnerException.Message + Environment.NewLine +
                e.GetException().InnerException.StackTrace);
        }
    }
}
