using System;
using System.Diagnostics;
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
                var worker = new Worker();
                await worker.RunAsync(args);
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
    }
}
