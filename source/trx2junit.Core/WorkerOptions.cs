// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;

namespace gfoidl.Trx2Junit.Core;

public class WorkerOptions
{
    public IList<string> InputFiles { get; set; }
    public string? OutputDirectory  { get; }
    public bool ConvertToJunit      { get; }
    //-------------------------------------------------------------------------
    public WorkerOptions(IList<string> inputFiles, string? outputDirectory = null, bool convertToJunit = true)
    {
        this.InputFiles      = inputFiles ?? throw new ArgumentNullException(nameof(inputFiles));
        this.OutputDirectory = outputDirectory;
        this.ConvertToJunit  = convertToJunit;
    }
    //-------------------------------------------------------------------------
    internal static WorkerOptions Parse(string[] args)
    {
        var inputFiles          = new List<string>();
        string? outputDirectory = null;
        bool convertToJunit     = true;

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
            else
            {
                inputFiles.Add(args[i]);
            }
        }

        return new WorkerOptions(inputFiles, outputDirectory, convertToJunit);
    }
}
