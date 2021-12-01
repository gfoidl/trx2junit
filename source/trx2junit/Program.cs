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
    Worker worker                   = new();
    worker.WorkerNotification      += static (s, e) => Console.WriteLine(e.Message);
    worker.WorkerErrorNotification += OnErrorNotification;
    WorkerOptions options           = WorkerOptions.Parse(args);

    await worker.RunAsync(options);
}
catch (Exception ex) when (!Debugger.IsAttached)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
    Environment.Exit(1);
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
static void OnErrorNotification(object? sender, WorkerNotificationEventArgs e)
{
    Debug.Assert(sender is not null);
    lock (sender)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(e.Message);
        Console.ResetColor();

        Environment.ExitCode = 1;
    }
}
//-----------------------------------------------------------------------------
[ExcludeFromCodeCoverage]
internal partial class Program { }
