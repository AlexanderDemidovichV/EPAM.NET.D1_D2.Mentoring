using System.IO;

namespace EPAM.Mentoring.Interfaces
{
    public interface IWatcher
    {
        void StartListen();
        void OnChanged(object source, FileSystemEventArgs e);
        void OnCreated(object source, FileSystemEventArgs e);
        void OnDelete(object source, FileSystemEventArgs e);
        void OnRenamed(object source, RenamedEventArgs e);
        void OnError(object source, ErrorEventArgs e);
    }
}
