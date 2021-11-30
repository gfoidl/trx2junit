// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using System.IO;

namespace gfoidl.Trx2Junit.Core.Abstractions;

public interface IFileSystem
{
    Stream OpenRead(string path);
    void CreateDirectory(string directory);
    IEnumerable<string> EnumerateFiles(string path, string pattern);
}
