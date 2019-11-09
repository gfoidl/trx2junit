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
        public Worker(IFileSystem? fileSystem = null, IGlobHandler? globHandler = null)
        {
            _fileSystem  = fileSystem  ?? new FileSystem();
            _globHandler = globHandler ?? new GlobHandler(_fileSystem);
        }
        //---------------------------------------------------------------------
        public async Task RunAsync(WorkerOptions? options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            _globHandler.ExpandWildcards(options);

            ITestResultXmlConverter converter;

            if (options.ConvertToJunit)
            {
                converter = new Trx2JunitConverter();
                Console.WriteLine($"Converting {options.InputFiles.Count} trx file(s) to JUnit-xml...");
            }
            else
            {
                converter = new Junit2TrxConverter();
                Console.WriteLine($"Converting {options.InputFiles.Count} junit file(s) to trx-xml...");
            }
            DateTime start = DateTime.Now;

            await Task.WhenAll(options.InputFiles.Select(input => this.Convert(input, options.OutputDirectory, converter)));

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal async Task Convert(string inputFile, string? outputPath = null, ITestResultXmlConverter? converter = null)
        {
            converter       ??= new Trx2JunitConverter();
            string outputFile = converter.GetOutputFile(inputFile, outputPath);
            this.EnsureOutputDirectoryExists(outputFile);

            Console.WriteLine($"Converting '{inputFile}' to '{outputFile}'");

            using (Stream input      = _fileSystem.OpenRead(inputFile))
            using (TextWriter output = new StreamWriter(outputFile, false, s_utf8))
            {
                await converter.ConvertAsync(input, output);
            }
        }
        //---------------------------------------------------------------------
        private void EnsureOutputDirectoryExists(string outputFile)
        {
            string? directory = Path.GetDirectoryName(outputFile);

            if (!string.IsNullOrWhiteSpace(directory))
                _fileSystem.CreateDirectory(directory);
        }
    }
}
