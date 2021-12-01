// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using System.IO;
using gfoidl.Trx2Junit.Core.Abstractions;

namespace gfoidl.Trx2Junit.Core;

/// <inheritdoc/>
public class FileSystem : IFileSystem
{
    /// <inheritdoc/>
    public Stream OpenRead(string path) => File.OpenRead(path);
    //-------------------------------------------------------------------------
    /// <inheritdoc/>
    public void CreateDirectory(string directory) => Directory.CreateDirectory(directory);
    //-------------------------------------------------------------------------
    /// <inheritdoc/>
    public IEnumerable<string> EnumerateFiles(string path, string pattern) => Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly);
}
