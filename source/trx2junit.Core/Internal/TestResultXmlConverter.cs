// (c) gfoidl, all rights reserved

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Abstractions;

namespace gfoidl.Trx2Junit.Core.Internal;

internal abstract class TestResultXmlConverter<TIn, TOut> : ITestResultXmlConverter
    where TIn  : Models.Test
    where TOut : Models.Test
{
    protected abstract Func<XElement, ITestResultXmlParser<TIn>> ParserFactory { get; }
    protected abstract Func<TIn, ITestConverter<TIn, TOut>> ConverterFactory   { get; }
    protected abstract Func<TOut, ITestResultXmlBuilder<TOut>> BuilderFactory  { get; }
    protected abstract string Extension                                        { get; }
    //-------------------------------------------------------------------------
    public virtual async Task ConvertAsync(Stream input, TextWriter output)
    {
        XElement testXml = await XElement.LoadAsync(input, LoadOptions.None, CancellationToken.None).ConfigureAwait(false);

        ITestResultXmlParser<TIn> parser = this.ParserFactory(testXml);
        parser.Parse();
        TIn sourceTest = parser.Result;

        ITestConverter<TIn, TOut> testConverter = this.ConverterFactory(sourceTest);
        testConverter.Convert();
        TOut targetTest = testConverter.Result;

        ITestResultXmlBuilder<TOut> builder = this.BuilderFactory(targetTest);
        builder.Build();

        await builder.Result.SaveAsync(output, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
    }
    //-------------------------------------------------------------------------
    public string GetOutputFile(string inputFile, string? outputPath = null)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(inputFile);
#else
        if (inputFile == null) throw new ArgumentNullException(nameof(inputFile));
#endif

        string outputFile = Path.ChangeExtension(inputFile, this.Extension);

        if (outputPath == null)
            return outputFile;

        string fileName = Path.GetFileName(outputFile);
        return Path.Combine(outputPath, fileName);
    }
}
