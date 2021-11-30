// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using System.IO;
using gfoidl.Trx2Junit.Core.Abstractions;

namespace gfoidl.Trx2Junit.Core.Internal;

internal class FileSystem : IFileSystem
{
    public Stream OpenRead(string path)                                    => File.OpenRead(path);
    public void CreateDirectory(string directory)                          => Directory.CreateDirectory(directory);
    public IEnumerable<string> EnumerateFiles(string path, string pattern) => Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
}
