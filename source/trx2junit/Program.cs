// (c) gfoidl, all rights reserved

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using gfoidl.Trx2Junit.Core;

PrintInfo();

if (args.Length < 1)
{
    Console.WriteLine("first arg must be the trx-file");
    Environment.Exit(1);
}

try
{
    Worker worker         = new();
    WorkerOptions options = WorkerOptions.Parse(args);
    await worker.RunAsync(options);
}
catch (Exception ex) when (!Debugger.IsAttached)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
    Environment.Exit(2);
}
//-----------------------------------------------------------------------------
static void PrintInfo()
{
    Version version = typeof(Program).Assembly.GetName().Version!;

    Console.WriteLine($"trx2junit (c) gfoidl -- v{version.Major}.{version.Minor}.{version.Revision}");
    Console.WriteLine("https://github.com/gfoidl/trx2junit");
    Console.WriteLine();
}
//-----------------------------------------------------------------------------
[ExcludeFromCodeCoverage]
internal partial class Program { }
