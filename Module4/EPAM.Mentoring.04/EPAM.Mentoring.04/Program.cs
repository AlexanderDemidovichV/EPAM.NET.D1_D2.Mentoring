using System;
using System.IO;

namespace EPAM.Mentoring
{
    public class Program
    {
        static void Main(string[] args)
        {
            var t = new Watcher();
            t.StartListen();
            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }
    }
}
