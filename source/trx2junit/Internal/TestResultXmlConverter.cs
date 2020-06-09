using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace trx2junit
{
    public abstract class TestResultXmlConverter<TIn, TOut> : ITestResultXmlConverter
        where TIn  : Models.Test
        where TOut : Models.Test
    {
        protected abstract bool InputIsTrx                                         { get; }
        protected abstract Func<XElement, ITestResultXmlParser<TIn>> ParserFactory { get; }
        protected abstract Func<TIn, ITestConverter<TIn, TOut>> ConverterFactory   { get; }
        protected abstract Func<TOut, ITestResultXmlBuilder<TOut>> BuilderFactory  { get; }
        protected abstract string Extension                                        { get; }
        //---------------------------------------------------------------------
        public void ValidateAgainstSchema(string? inputFile)
        {
            Debug.Assert(inputFile != null);

            if (ValidationHelper.TryValidateXml(inputFile, validateJunit: !this.InputIsTrx, out string? msg))
            {
                return;
            }

            string exMsg = this.InputIsTrx
                ? "Given xml file is not a valid trx file"
                : "Given xml file is not a valid junit file";

            throw new Exception(exMsg + "\n" + msg);
        }
        //---------------------------------------------------------------------
        public virtual async Task ConvertAsync(Stream? input, TextWriter? output)
        {
            XElement testXml = await XElement.LoadAsync(input, LoadOptions.None, CancellationToken.None);

            ITestResultXmlParser<TIn> parser = this.ParserFactory(testXml);
            parser.Parse();
            TIn sourceTest = parser.Result;

            ITestConverter<TIn, TOut> testConverter = this.ConverterFactory(sourceTest);
            testConverter.Convert();
            TOut targetTest = testConverter.Result;

            ITestResultXmlBuilder<TOut> builder = this.BuilderFactory(targetTest);
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
