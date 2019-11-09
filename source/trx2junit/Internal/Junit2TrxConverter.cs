using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Junit2TrxConverter : TestResultXmlConverter
    {
        protected override Func<XElement, ITestResultXmlParser> ParserFactory => testXml => new JunitParser(testXml);
        protected override Func<Test, ITestResultXmlBuilder> BuilderFactory   => test    => new TrxBuilder(test);
        protected override string Extension                                   => "trx";
    }
}
