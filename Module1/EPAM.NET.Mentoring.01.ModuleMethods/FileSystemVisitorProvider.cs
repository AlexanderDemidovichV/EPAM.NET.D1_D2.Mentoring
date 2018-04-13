using System;
using System.IO;

namespace EPAM.NET.Mentoring
{
    public class FileSystemVisitorProvider : IFileSystemVisitorProvider
    {
        public string[] GetDirectories(string root)
        {
            return GetEntity(root, Directory.GetDirectories);
        }

        public string[] GetFiles(string root)
        {
            return GetEntity(root, Directory.GetFiles);
        }

        private string[] GetEntity(string root, Func<string, string[]> method)
        {
            try {
                return method(root);
            } catch (UnauthorizedAccessException e) {
                throw new FileSystemVisitorException(e.Message, e);
            } catch (ArgumentException e) {
                throw new FileSystemVisitorException(e.Message, e);
            } catch (PathTooLongException e) {
                throw new FileSystemVisitorException(e.Message, e);
            }catch (DirectoryNotFoundException e) {
                throw new FileSystemVisitorException(e.Message, e);
            }catch (IOException e) {
                throw new FileSystemVisitorException(e.Message, e);
            }
        }
    }
}
