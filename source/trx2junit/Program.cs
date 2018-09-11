using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace trx2junit
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("first arg must be the trx-file");
                Environment.Exit(1);
            }

            try
            {
                await RunAsync(args);
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();
                Environment.Exit(2);
            }

            if (Debugger.IsAttached)
                Console.ReadKey();
        }
        //---------------------------------------------------------------------
        private static async Task RunAsync(string[] args)
        {
            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.WriteLine($"Converting {args.Length } trx file(s) to JUnit-xml...");
            DateTime start = DateTime.Now;

            await Task.WhenAll(args.Select(Convert));

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }

        private static async Task Convert(string trxFile){
            string jUnitFile = Path.ChangeExtension(trxFile, "xml");
            Console.WriteLine($"Converting '{trxFile}' to '{jUnitFile}'");
            var utf8 = new UTF8Encoding(false);
            using (Stream input      = File.OpenRead(trxFile))
            using (TextWriter output = new StreamWriter(jUnitFile, false, utf8))
            {
                var converter = new Trx2JunitConverter();
                await converter.Convert(input, output);
            }
        }
    }
}
