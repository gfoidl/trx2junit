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
        //---------------------------------------------------------------------
        public async Task RunAsync(string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"Converting {args.Length } trx file(s) to JUnit-xml...");
            DateTime start = DateTime.Now;

            await Task.WhenAll(args.Select(this.Convert));

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal async Task Convert(string trxFile)
        {
            string jUnitFile = Path.ChangeExtension(trxFile, "xml");

            Console.WriteLine($"Converting '{trxFile}' to '{jUnitFile}'");

            using (Stream input      = File.OpenRead(trxFile))
            using (TextWriter output = new StreamWriter(jUnitFile, false, s_utf8))
            {
                var converter = new Trx2JunitConverter();
                await converter.Convert(input, output);
            }
        }
    }
}
