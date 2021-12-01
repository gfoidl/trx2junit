// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Abstractions;
using gfoidl.Trx2Junit.Core.Models.JUnit;
using gfoidl.Trx2Junit.Core.Models.Trx;

namespace gfoidl.Trx2Junit.Core.Internal;

internal class Junit2TrxTestResultXmlConverter : TestResultXmlConverter<JUnitTest, TrxTest>
{
    protected override Func<XElement, ITestResultXmlParser<JUnitTest>> ParserFactory        => testXml => new JUnitTestResultXmlParser(testXml);
    protected override Func<JUnitTest, ITestConverter<JUnitTest, TrxTest>> ConverterFactory => test    => new JUnit2TrxTestConverter(test);
    protected override Func<TrxTest, ITestResultXmlBuilder<TrxTest>> BuilderFactory         => test    => new TrxTestResultXmlBuilder(test);
    protected override string Extension                                                     => "trx";
}
