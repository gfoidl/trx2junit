using System;
using System.Collections.Generic;
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
        public void ExpandWildcards(WorkerOptions options) => throw new NotImplementedException();
    }
}
