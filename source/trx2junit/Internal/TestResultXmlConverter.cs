using System;
using System.IO;
using System.Threading.Tasks;

namespace trx2junit
{
    public abstract class TestResultXmlConverter : ITestResultXmlConverter
    {
        protected abstract string Extension { get; }
        //---------------------------------------------------------------------
        public abstract Task ConvertAsync(Stream? input, TextWriter? output);
        //---------------------------------------------------------------------
        public string GetOutputFile(string? inputFile, string? outputPath = null)
        {
            if (inputFile == null) throw new ArgumentNullException(nameof(inputFile));

            string outputFile = Path.ChangeExtension(inputFile, this.Extension);

            if (outputPath == null)
                return outputFile;

            string fileName = Path.GetFileName(outputFile);
            return Path.Combine(outputPath, fileName);
        }
    }
}
