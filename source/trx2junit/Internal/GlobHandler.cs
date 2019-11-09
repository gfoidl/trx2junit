using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace trx2junit
{
    public class GlobHandler : IGlobHandler
    {
        private readonly IFileSystem _fileSystem;
        //---------------------------------------------------------------------
        public GlobHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }
        //---------------------------------------------------------------------
        public void ExpandWildcards(WorkerOptions? options)
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
        //---------------------------------------------------------------------
        private void Expand(string input, List<string> expandedFiles)
        {
            string? path   = Path.GetDirectoryName(input);
            Debug.Assert(path != null);
            string pattern = Path.GetFileName(input);

            IEnumerable<string> files = _fileSystem.EnumerateFiles(path, pattern);

            expandedFiles.AddRange(files);
        }
    }
}
