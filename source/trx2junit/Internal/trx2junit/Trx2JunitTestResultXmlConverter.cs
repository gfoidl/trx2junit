using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Trx2JunitTestResultXmlConverter : TestResultXmlConverter<TrxTest, JUnitTest>
    {
        protected override Func<XElement, ITestResultXmlParser<TrxTest>> ParserFactory        => testXml => new TrxTestResultXmlParser(testXml);
        protected override Func<TrxTest, ITestConverter<TrxTest, JUnitTest>> ConverterFactory => test    => new Trx2JunitTestConverter(test);
        protected override Func<JUnitTest, ITestResultXmlBuilder<JUnitTest>> BuilderFactory   => test    => new JUnitTestResultXmlBuilder(test);
        protected override string Extension                                                   => "xml";
    }
}
