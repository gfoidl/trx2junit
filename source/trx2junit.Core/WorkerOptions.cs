// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;

namespace gfoidl.Trx2Junit.Core;

/// <summary>
/// The settings / configuration for the <see cref="Worker"/>.
/// </summary>
public class WorkerOptions
{
    /// <summary>
    /// The list of input files that should be converted.
    /// </summary>
    public IList<string> InputFiles { get; internal set; }

    /// <summary>
    /// The directory where the converted files are stored.
    /// </summary>
    public string? OutputDirectory { get; }

    /// <summary>
    /// Indicates the direction of conversion. When <c>true</c> the conversion is
    /// trx to junit. When <c>false</c> the conversion is junit to trx.
    /// </summary>
    public bool ConvertToJunit { get; }

    /// <summary>
    /// If set to <c>true</c>, then the JUnit message attributes will be emitted
    /// to system-out too.
    /// </summary>
    public bool JUnitMessagesToSystemErr { get; }
    //-------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of <see cref="WorkerOptions"/>.
    /// </summary>
    /// <param name="inputFiles">See <see cref="InputFiles"/>.</param>
    /// <param name="outputDirectory">See <see cref="OutputDirectory"/>.</param>
    /// <param name="convertToJunit">See <see cref="ConvertToJunit"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="inputFiles"/> is <c>null</c>.
    /// </exception>
    public WorkerOptions(
        IList<string> inputFiles,
        string?       outputDirectory          = null,
        bool          convertToJunit           = true,
        bool          junitMessagesToSystemErr = false)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(inputFiles);
        this.InputFiles = inputFiles;
#else
        this.InputFiles      = inputFiles ?? throw new ArgumentNullException(nameof(inputFiles));
#endif
        this.OutputDirectory          = outputDirectory;
        this.ConvertToJunit           = convertToJunit;
        this.JUnitMessagesToSystemErr = junitMessagesToSystemErr;
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// Parses the given arguments into a <see cref="WorkerOptions"/>.
    /// </summary>
    /// <param name="args">The args (as given by the command line) to parse.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="args"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <c>--output</c> is given as argument, but no value following.
    /// </exception>
    public static WorkerOptions Parse(string[] args)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(args);
#else
        if (args is null) throw new ArgumentNullException(nameof(args));
#endif
        List<string> inputFiles       = new();
        string? outputDirectory       = null;
        bool convertToJunit           = true;
        bool junitMessagesToSystemErr = false;

        for (int i = 0; i < args.Length; ++i)
        {
            if (args[i] == "--output")
            {
                if (i + 1 < args.Length)
                {
                    i++;
                    outputDirectory = args[i];
                }
                else
                {
                    throw new ArgumentException(Resources.Strings.Args_Output_no_Value);
                }
            }
            else if (args[i] == "--junit2trx")
            {
                convertToJunit = false;
            }
            else if (args[i] == "--junit-messages-to-system-err")
            {
                junitMessagesToSystemErr = true;
            }
            else
            {
                inputFiles.Add(args[i]);
            }
        }

        return new WorkerOptions(inputFiles, outputDirectory, convertToJunit, junitMessagesToSystemErr);
    }
}
