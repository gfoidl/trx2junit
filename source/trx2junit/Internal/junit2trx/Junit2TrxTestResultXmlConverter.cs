using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Junit2TrxTestResultXmlConverter : TestResultXmlConverter<JUnitTest, TrxTest>
    {
        protected override bool InputIsTrx                                                      => false;
        protected override Func<XElement, ITestResultXmlParser<JUnitTest>> ParserFactory        => testXml => new JUnitTestResultXmlParser(testXml);
        protected override Func<JUnitTest, ITestConverter<JUnitTest, TrxTest>> ConverterFactory => test    => new JUnit2TrxTestConverter(test);
        protected override Func<TrxTest, ITestResultXmlBuilder<TrxTest>> BuilderFactory         => test    => new TrxTestResultXmlBuilder(test);
        protected override string Extension                                                     => "trx";
    }
}
