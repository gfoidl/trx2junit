using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trx2junit
{
    public abstract class TestResultXmlConverter : ITestResultXmlConverter
    {
        protected abstract Func<XElement, ITestResultXmlParser> ParserFactory      { get; }
        protected abstract Func<Models.Test, ITestResultXmlBuilder> BuilderFactory { get; }
        protected abstract string Extension                                        { get; }
        //---------------------------------------------------------------------
        public virtual async Task ConvertAsync(Stream? input, TextWriter? output)
        {
            XElement testXml = await XElement.LoadAsync(input, LoadOptions.None, CancellationToken.None);

            ITestResultXmlParser parser = this.ParserFactory(testXml);
            parser.Parse();

            ITestResultXmlBuilder builder = this.BuilderFactory(parser.Result);
            builder.Build();

            await builder.Result.SaveAsync(output, SaveOptions.None, CancellationToken.None);
        }
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
