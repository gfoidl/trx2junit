using System.IO;

namespace trx2junit
{
    public interface IFileSystem
    {
        Stream OpenRead(string path);
        void CreateDirectory(string directory);
    }
}
