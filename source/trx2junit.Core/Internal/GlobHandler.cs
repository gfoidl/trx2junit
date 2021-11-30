// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using gfoidl.Trx2Junit.Core.Abstractions;

namespace gfoidl.Trx2Junit.Core.Internal;

internal class GlobHandler : IGlobHandler
{
    private readonly IFileSystem _fileSystem;
    //-------------------------------------------------------------------------
    public GlobHandler(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }
    //-------------------------------------------------------------------------
    public void ExpandWildcards(WorkerOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        var expandedFiles = new List<string>();

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
