using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.TrxTestResultXmlBuilderTests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        [TestCase("./data/junit/mstest.xml"           , 3, 1, 0)]
        [TestCase("./data/junit/mstest-datadriven.xml", 5, 2, 0)]
        [TestCase("./data/junit/mstest-ignore.xml"    , 4, 1, 0)]
        [TestCase("./data/junit/nunit.xml"            , 3, 1, 0)]
        [TestCase("./data/junit/nunit-datadriven.xml" , 5, 2, 0)]
        [TestCase("./data/junit/nunit-ignore.xml"     , 5, 1, 0)]
        [TestCase("./data/junit/nunit-memberdata.xml" , 5, 2, 0)]
        [TestCase("./data/junit/xunit.xml"            , 3, 1, 0)]
        [TestCase("./data/junit/xunit-datadriven.xml" , 5, 2, 0)]
        [TestCase("./data/junit/xunit-ignore.xml"     , 4, 1, 0)]
        [TestCase("./data/junit/xunit-memberdata.xml" , 5, 2, 0)]
        [TestCase("./data/junit/yanlend.xml"          , 3, 0, 1)]
        public void File_given___correct_counts(string junitFile, int expectedTestCount, int expectedFailureCount, int? expectedErrorCount)
        {
            XElement junit = XElement.Load(junitFile);
            var parser     = new JUnitTestResultXmlParser(junit);

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
                Assert.AreEqual(expectedErrorCount  , xCounters.ReadInt("error") , nameof(expectedErrorCount));
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void JUnit_testcase_with_stdout_and_stderr___system_out_and_system_err_set()
        {
            XElement junit = XElement.Load("./data/junit/with-system-out.xml");
            var parser     = new JUnitTestResultXmlParser(junit);

            parser.Parse();
            Models.JUnitTest testData = parser.Result;

            var converter = new JUnit2TrxTestConverter(testData);
            converter.Convert();

            Models.TrxTest trxTest = converter.Result;
            var sut                = new TrxTestResultXmlBuilder(trxTest);
            sut.Build();

            XElement xResults        = sut.Result.Element(TrxBase.s_XN + "Results");
            XElement xUnitTestResult = xResults.Elements(TrxBase.s_XN + "UnitTestResult").Single();
            XElement xOutput         = xUnitTestResult.Element(TrxBase.s_XN + "Output");
            XElement xStdOut         = xOutput.Element(TrxBase.s_XN + "StdOut");
            XElement xStdErr         = xOutput.Element(TrxBase.s_XN + "StdErr");

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(xStdOut);
                Assert.IsNotNull(xStdErr);
            });

            Assert.Multiple(() =>
            {
                Assert.AreEqual("message written to system-out", xStdOut.Value);
                Assert.AreEqual("message written to system-err", xStdErr.Value);
            });
        }
    }
}
