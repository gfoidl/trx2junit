using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace trx2junit
{
    public class Worker
    {
        private static readonly Encoding s_utf8 = new UTF8Encoding(false);

        private readonly IFileSystem  _fileSystem;
        private readonly IGlobHandler _globHandler;
        //---------------------------------------------------------------------
        public Worker(IFileSystem fileSystem = null, IGlobHandler globHandler = null)
        {
            _fileSystem  = fileSystem  ?? new FileSystem();
            _globHandler = globHandler ?? new GlobHandler(_fileSystem);
        }
        //---------------------------------------------------------------------
        public async Task RunAsync(WorkerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            _globHandler.ExpandWildcards(options);

            Console.WriteLine($"Converting {options.InputFiles.Count} trx file(s) to JUnit-xml...");
            DateTime start = DateTime.Now;

            await Task.WhenAll(options.InputFiles.Select(trx => this.Convert(trx, options.OutputDirectory)));

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal async Task Convert(string trxFile, string outputPath = null)
        {
            string jUnitFile = GetJunitFile(trxFile, outputPath);
            this.EnsureOutputDirectoryExists(jUnitFile);

            Console.WriteLine($"Converting '{trxFile}' to '{jUnitFile}'");

            using (Stream input      = _fileSystem.OpenRead(trxFile))
            using (TextWriter output = new StreamWriter(jUnitFile, false, s_utf8))
            {
                var converter = new Trx2JunitConverter();
                await converter.Convert(input, output);
            }
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal static string GetJunitFile(string trxFile, string outputPath = null)
        {
            string junitFile = Path.ChangeExtension(trxFile, "xml");

            if (outputPath == null)
                return junitFile;

            string fileName = Path.GetFileName(junitFile);
            return Path.Combine(outputPath, fileName);
        }
        //---------------------------------------------------------------------
        private void EnsureOutputDirectoryExists(string jUnitFile)
        {
            string directory = Path.GetDirectoryName(jUnitFile);

            if (!string.IsNullOrWhiteSpace(directory))
                _fileSystem.CreateDirectory(directory);
        }
    }
}
