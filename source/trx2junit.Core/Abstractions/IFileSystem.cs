// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using System.IO;

namespace gfoidl.Trx2Junit.Core.Abstractions;

/// <summary>
/// Abstraction of the filesystem.
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Opens a file, given by <paramref name="path"/> for reading.
    /// </summary>
    /// <param name="path">The path that points to the file to open for reading.</param>
    /// <returns>The opened stream that allows reading from the given file.</returns>
    /// <remarks>
    /// The <paramref name="path"/> follows the rules of .NET.
    /// <para>
    /// For possible exceptions see <see cref="File.OpenRead(string)"/>.
    /// </para>
    /// </remarks>
    Stream OpenRead(string path);
    //-------------------------------------------------------------------------
    /// <summary>
    /// Creates a directory given by <paramref name="directory"/>.
    /// </summary>
    /// <param name="directory">The directory to create.</param>
    /// <remarks>
    /// For possible exceptions see <see cref="Directory.CreateDirectory(string)"/>.
    /// </remarks>
    void CreateDirectory(string directory);
    //-------------------------------------------------------------------------
    /// <summary>
    /// Enumerates the files in <paramref name="path"/> according a pattern
    /// given by <paramref name="pattern"/>.
    /// </summary>
    /// <param name="path">The path where the files should be enumerated.</param>
    /// <param name="pattern">The pattern to enumerate the files.</param>
    /// <returns>The enumerates files.</returns>
    /// <remarks>
    /// For possible exceptions see <see cref="Directory.EnumerateDirectories(string, string, SearchOption)"/>.
    /// </remarks>
    IEnumerable<string> EnumerateFiles(string path, string pattern);
}
