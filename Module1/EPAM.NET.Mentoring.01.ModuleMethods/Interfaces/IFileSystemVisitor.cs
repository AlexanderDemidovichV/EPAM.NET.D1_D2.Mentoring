using System;
using System.Collections.Generic;

namespace EPAM.NET.Mentoring
{
    public interface IFileSystemVisitor
    {
        event EventHandler<FileSystemEventArgs> StartWalk;
        event EventHandler<FileSystemEventArgs> FinishWalk;

        event EventHandler<FileSystemEventArgs> FileFinded;
        event EventHandler<FileSystemEventArgs> DirectoryFinded;

        event EventHandler<FileSystemEventArgs> FilteredFileFinded;
        event EventHandler<FileSystemEventArgs> FilteredDirectoryFinded;

        IEnumerable<string> Visit(string root);

        bool Cancel { get; set; }

        bool SkipNext { get; set; }
    }
}
