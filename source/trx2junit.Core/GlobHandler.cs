// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using gfoidl.Trx2Junit.Core.Abstractions;

namespace gfoidl.Trx2Junit.Core;

/// <inheritdoc/>
public class GlobHandler : IGlobHandler
{
    private readonly IFileSystem _fileSystem;
    //-------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of the <see cref="GlobHandler"/>.
    /// </summary>
    /// <param name="fileSystem">The implementation of the filesystem.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="fileSystem"/> is <c>null</c>.
    /// </exception>
    public GlobHandler(IFileSystem fileSystem)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(fileSystem);
        _fileSystem = fileSystem;
#else
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
#endif
    }
    //-------------------------------------------------------------------------
    /// <inheritdoc/>
    public void ExpandWildcards(WorkerOptions options)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(options);
#else
        if (options is null) throw new ArgumentNullException(nameof(options));
#endif

        List<string> expandedFiles = new();

        foreach (string inpupt in options.InputFiles)
        {
            if (!inpupt.Contains('*'))
            {
                expandedFiles.Add(inpupt);
                continue;
            }

            this.Expand(inpupt, expandedFiles);
        }

        options.InputFiles = expandedFiles;
    }
    //-------------------------------------------------------------------------
    private void Expand(string input, List<string> expandedFiles)
    {
        string? path    = Path.GetDirectoryName(input);
        string? pattern = Path.GetFileName(input);

        Debug.Assert(path    is not null);
        Debug.Assert(pattern is not null);

        IEnumerable<string> files = _fileSystem.EnumerateFiles(path, pattern);

        expandedFiles.AddRange(files);
    }
}
