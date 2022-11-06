// (c) gfoidl, all rights reserved

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gfoidl.Trx2Junit.Core.Abstractions;
using gfoidl.Trx2Junit.Core.Internal;

namespace gfoidl.Trx2Junit.Core;

/// <summary>
/// The worker that acts as entry for the conversion of test-files.
/// </summary>
public class Worker
{
    private static readonly Encoding s_utf8 = new UTF8Encoding(false);

    private readonly IFileSystem  _fileSystem;
    private readonly IGlobHandler _globHandler;

    /// <summary>
    /// Event that is fired when the worker triggers a notification.
    /// </summary>
    public event EventHandler<WorkerNotificationEventArgs>? WorkerNotification;

    /// <summary>
    /// Event that is fired when the worker triggers an error.
    /// </summary>
    public event EventHandler<WorkerNotificationEventArgs>? WorkerErrorNotification;
    //-------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of <see cref="Worker"/>.
    /// </summary>
    /// <param name="fileSystem">
    /// Implementation of the <see cref="IFileSystem"/>.
    /// When <c>null</c> is passed, an default internal implementation is used.
    /// </param>
    /// <param name="globHandler">
    /// Implementation of the <see cref="IGlobHandler"/>.
    /// When <c>null</c> is passed, an default internal implementation is used.
    /// </param>
    public Worker(IFileSystem? fileSystem = null, IGlobHandler? globHandler = null)
    {
        _fileSystem  = fileSystem  ?? new FileSystem();
        _globHandler = globHandler ?? new GlobHandler(_fileSystem);
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// Executes the conversion of test-files asynchronously.
    /// </summary>
    /// <param name="options">The <see cref="WorkerOptions"/> to use for the conversion.</param>
    /// <returns>A task that completes when the conversion is finished.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="options"/> is <c>null</c>.
    /// </exception>
    public async Task RunAsync(WorkerOptions options)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(options);
#else
        if (options is null) throw new ArgumentNullException(nameof(options));
#endif

        CultureInfo currentThreadCulture      = Thread.CurrentThread.CurrentCulture;
        CultureInfo currentUICulture          = Thread.CurrentThread.CurrentUICulture;
        Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        try
        {
            _globHandler.ExpandWildcards(options);

            ITestResultXmlConverter converter;

            if (options.ConvertToJunit)
            {
                converter = new Trx2JunitTestResultXmlConverter();
                this.OnNotification($"Converting {options.InputFiles.Count} trx file(s) to JUnit-xml...");
            }
            else
            {
                converter = new Junit2TrxTestResultXmlConverter();
                this.OnNotification($"Converting {options.InputFiles.Count} junit file(s) to trx-xml...");
            }
            DateTime start = DateTime.Now;

            await Task.WhenAll(options.InputFiles.Select(input => this.ConvertAsync(converter, input, options.OutputDirectory))).ConfigureAwait(false);
            this.OnNotification($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }
        finally
        {
            Thread.CurrentThread.CurrentCulture   = currentThreadCulture;
            Thread.CurrentThread.CurrentUICulture = currentUICulture;
        }
    }
    //-------------------------------------------------------------------------
    // internal for testing
    internal async Task ConvertAsync(ITestResultXmlConverter converter, string inputFile, string? outputPath = null)
    {
        string outputFile = converter.GetOutputFile(inputFile, outputPath);

        this.EnsureOutputDirectoryExists(outputFile);
        this.OnNotification($"Converting '{inputFile}' to '{outputFile}'");

        using Stream input      = _fileSystem.OpenRead(inputFile);
        using TextWriter output = new StreamWriter(outputFile, false, s_utf8);

        try
        {
            await converter.ConvertAsync(input, output).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.OnErrorNotification(ex.Message);
        }
    }
    //-------------------------------------------------------------------------
    private void EnsureOutputDirectoryExists(string outputFile)
    {
        string? directory = Path.GetDirectoryName(outputFile);

        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            _fileSystem.CreateDirectory(directory);
    }
    //-------------------------------------------------------------------------
    private void OnNotification(string message)
    {
        this.WorkerNotification?.Invoke(this, new WorkerNotificationEventArgs(message));
    }
    //-------------------------------------------------------------------------
    private void OnErrorNotification(string message)
    {
        this.WorkerErrorNotification?.Invoke(this, new WorkerNotificationEventArgs(message));
    }
}
