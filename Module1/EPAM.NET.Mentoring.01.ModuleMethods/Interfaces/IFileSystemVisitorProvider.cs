using System.Runtime.Serialization;

namespace EPAM.NET.Mentoring
{
    public interface IFileSystemVisitorProvider
    {
        string[] GetDirectories(string root);

        string[] GetFiles(string root);
    }
}
