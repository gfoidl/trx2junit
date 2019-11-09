using System;
using System.Collections.Generic;

namespace trx2junit
{
    public class WorkerOptions
    {
        public IList<string> InputFiles { get; set; }
        public string? OutputDirectory  { get; }
        //---------------------------------------------------------------------
        public WorkerOptions(IList<string>? inputFiles, string? outputDirectory = null)
        {
            this.InputFiles      = inputFiles ?? throw new ArgumentNullException(nameof(inputFiles));
            this.OutputDirectory = outputDirectory;
        }
        //---------------------------------------------------------------------
        internal static WorkerOptions Parse(string[] args)
        {
            var inputFiles          = new List<string>();
            string? outputDirectory = null;

            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == "--output")
                {
                    if (i + 1 < args.Length)
                    {
                        i++;
                        outputDirectory = args[i];
                        continue;
                    }
                    else
                    {
                        throw new ArgumentException(Resources.Strings.Args_Output_no_Value);
                    }
                }

                inputFiles.Add(args[i]);
            }

            return new WorkerOptions(inputFiles, outputDirectory);
        }
    }
}
