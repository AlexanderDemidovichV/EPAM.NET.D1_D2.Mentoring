using System;
using EPAM.Mentoring.Exceptions;
using EPAM.Mentoring._04.Resources;

namespace EPAM.Mentoring
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
            };

            try {
                var watcher = new Watcher();
                watcher.StartListen();
            } catch (TypeInitializationException e) {
                throw new FileSystemWatcherException(Messages.TypeInitializationProblems, e);
            } finally {
                Console.WriteLine(Messages.ExitMessage);
                Console.ReadLine();
            }
        }
    }
}
