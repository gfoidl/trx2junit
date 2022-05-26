// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Abstractions;
using gfoidl.Trx2Junit.Core.Models.JUnit;
using gfoidl.Trx2Junit.Core.Models.Trx;

namespace gfoidl.Trx2Junit.Core.Internal;

internal sealed class Trx2JunitTestResultXmlConverter : TestResultXmlConverter<TrxTest, JUnitTest>
{
    private readonly JUnitOptions _jUnitOptions;
    //-------------------------------------------------------------------------
    public Trx2JunitTestResultXmlConverter(JUnitOptions jUnitOptions) => _jUnitOptions = jUnitOptions;
    //-------------------------------------------------------------------------
    protected override Func<XElement, ITestResultXmlParser<TrxTest>> ParserFactory        => testXml => new TrxTestResultXmlParser(testXml);
    protected override Func<TrxTest, ITestConverter<TrxTest, JUnitTest>> ConverterFactory => test    => new Trx2JunitTestConverter(test);
    protected override Func<JUnitTest, ITestResultXmlBuilder<JUnitTest>> BuilderFactory   => test    => new JUnitTestResultXmlBuilder(test, _jUnitOptions);
    protected override string Extension                                                   => "xml";
}
