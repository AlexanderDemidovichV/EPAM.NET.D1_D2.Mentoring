using System;
using System.Collections.Generic;


namespace EPAM.NET.Mentoring
{
    public class FileSystemVisitor: IFileSystemVisitor
    {
        private readonly Func<string, bool> _searchPattern;

        private readonly IFileSystemVisitorProvider _fileSystemVisitorProvider;

        public event EventHandler<FileSystemEventArgs> StartWalk;
        public event EventHandler<FileSystemEventArgs> FinishWalk;
        public event EventHandler<FileSystemEventArgs> FileFinded;
        public event EventHandler<FileSystemEventArgs> DirectoryFinded;
        public event EventHandler<FileSystemEventArgs> FilteredFileFinded;
        public event EventHandler<FileSystemEventArgs> FilteredDirectoryFinded;

        public bool Cancel { get; set; }

        public bool SkipNext { get; set; }


        public List<string> log = new List<string>();

        public FileSystemVisitor(IFileSystemVisitorProvider fileSystemVisitorProvider) : this(fileSystemVisitorProvider, s => true)
        {
        }

        public FileSystemVisitor(IFileSystemVisitorProvider fileSystemVisitorProvider, Func<string, bool> searchPattern)
        {
            _fileSystemVisitorProvider = fileSystemVisitorProvider;
            _searchPattern = searchPattern;
        }

        public IEnumerable<string> Visit(string root)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            OnStartWalk(new FileSystemEventArgs("start"));

            var result = new List<string>();

            foreach (string value in WalkDirectoryTree(root)) {
                result.Add(value);
            }

            OnFinishWalk(new FileSystemEventArgs("finish"));
            return result;
        }

        protected virtual void OnStartWalk(FileSystemEventArgs args)
        {
            StartWalk?.Invoke(this, args);
        }

        protected virtual void OnFinishWalk(FileSystemEventArgs args)
        {
            FinishWalk?.Invoke(this, args);
        }

        protected virtual void OnFilteredFileFinded(FileSystemEventArgs args)
        {
            FilteredFileFinded?.Invoke(this, args);
        }

        protected virtual void OnFilteredDirectoryFinded(FileSystemEventArgs args)
        {
            FilteredDirectoryFinded?.Invoke(this, args);
        }

        protected virtual void OnFileFinded(FileSystemEventArgs args)
        {
            FileFinded?.Invoke(this, args);
        }

        protected virtual void OnDirectoryFinded(FileSystemEventArgs args)
        {
            DirectoryFinded?.Invoke(this, args);
        }

        private IEnumerable<string> WalkDirectoryTree(string root)
        {
            string[] files = null;
            OnDirectoryFinded(new FileSystemEventArgs(root));

            if (Cancel)
                yield break;

            if (_searchPattern(root) && !SkipNext) {
                OnFilteredDirectoryFinded(new FileSystemEventArgs(root));

                if (Cancel)
                    yield break;

                if (!SkipNext) {
                    yield return root;

                    files = _fileSystemVisitorProvider.GetFiles(root);

                    foreach (var value in WalkThroughFiles(files)) {
                        yield return value;
                    }

                    foreach (var value in WalkThroughDirectories(root)) {
                        yield return value;
                    }
                }
            }
            SkipNext = false;
        }

        private IEnumerable<string> WalkThroughFiles(string[] files)
        {
            if (files != null) {
                foreach (string fi in files) {
                    OnFileFinded(new FileSystemEventArgs(fi));

                    if (Cancel)
                        yield break;

                    if (_searchPattern(fi) && !SkipNext) {
                        OnFilteredFileFinded(new FileSystemEventArgs(fi));

                        if (Cancel)
                            yield break;

                        if (!SkipNext)
                            yield return fi;
                    }
                    SkipNext = false;
                }
            }
        }

        private IEnumerable<string> WalkThroughDirectories(string root)
        {
            foreach (string dir in _fileSystemVisitorProvider.GetDirectories(root)) {
                foreach (var value in WalkDirectoryTree(dir)) {
                    yield return value;
                }
            }
        }
    }
}
