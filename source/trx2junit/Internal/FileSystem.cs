using System.Collections.Generic;
using System.IO;

namespace trx2junit
{
    public class FileSystem : IFileSystem
    {
        public Stream OpenRead(string path)
            => File.OpenRead(path);
        //---------------------------------------------------------------------
        public void CreateDirectory(string directory)
            => Directory.CreateDirectory(directory);
        //---------------------------------------------------------------------
        public IEnumerable<string> EnumerateFiles(string path, string pattern)
            => Directory.EnumerateFiles(path, path, SearchOption.TopDirectoryOnly);
    }
}
