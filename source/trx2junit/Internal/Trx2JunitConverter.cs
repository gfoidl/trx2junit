using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Trx2JunitConverter : TestResultXmlConverter
    {
        protected override Func<XElement, ITestResultXmlParser> ParserFactory => testXml => new TrxParser(testXml);
        protected override Func<Test, ITestResultXmlBuilder> BuilderFactory   => test    => new JUnitBuilder(test);
        protected override string Extension                                   => "xml";
    }
}
