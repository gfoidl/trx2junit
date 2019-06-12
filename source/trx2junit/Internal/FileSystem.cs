using System.IO;

namespace trx2junit
{
    public class FileSystem : IFileSystem
    {
        public Stream OpenRead(string path)           => File.OpenRead(path);
        public void CreateDirectory(string directory) => Directory.CreateDirectory(directory);
    }
}
