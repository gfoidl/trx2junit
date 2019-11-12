using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.TrxTestResultXmlBuilderTests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        [TestCase("./data/junit/mstest.xml"           , 3, 1)]
        [TestCase("./data/junit/mstest-datadriven.xml", 5, 2)]
        [TestCase("./data/junit/mstest-ignore.xml"    , 4, 1)]
        [TestCase("./data/junit/nunit.xml"            , 3, 1)]
        [TestCase("./data/junit/nunit-datadriven.xml" , 5, 2)]
        [TestCase("./data/junit/nunit-ignore.xml"     , 5, 1)]
        [TestCase("./data/junit/nunit-memberdata.xml" , 5, 2)]
        [TestCase("./data/junit/xunit.xml"            , 3, 1)]
        [TestCase("./data/junit/xunit-datadriven.xml" , 5, 2)]
        [TestCase("./data/junit/xunit-ignore.xml"     , 4, 1)]
        [TestCase("./data/junit/xunit-memberdata.xml" , 5, 2)]
        public void File_given___correct_counts(string junitFile, int expectedTestCount, int expectedFailureCount)
        {
            XElement trx = XElement.Load(junitFile);
            var parser   = new JUnitTestResultXmlParser(trx);

            parser.Parse();
            Models.JUnitTest testData = parser.Result;

            var converter = new JUnit2TrxTestConverter(testData);
            converter.Convert();

            Models.TrxTest trxTest = converter.Result;
            var sut                = new TrxTestResultXmlBuilder(trxTest);
            sut.Build();

            XElement xResultSummary = sut.Result.Element(TrxBase.s_XN + "ResultSummary");
            XElement xCounters      = xResultSummary.Element(TrxBase.s_XN + "Counters");

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedTestCount   , xCounters.ReadInt("total") , nameof(expectedTestCount));
                Assert.AreEqual(expectedFailureCount, xCounters.ReadInt("failed"), nameof(expectedFailureCount));
            });
        }
    }
}
