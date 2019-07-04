using System.Collections.Generic;
using System.IO;

#nullable enable

namespace trx2junit
{
    public interface IFileSystem
    {
        Stream OpenRead(string? path);
        void CreateDirectory(string? directory);
        IEnumerable<string> EnumerateFiles(string? path, string? pattern);
    }
}
