using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitTestResultXmlBuilderTests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        [TestCase("./data/trx/mstest.trx"           , 3, 1)]
        [TestCase("./data/trx/mstest-datadriven.trx", 5, 2)]
        [TestCase("./data/trx/mstest-ignore.trx"    , 4, 1)]
        [TestCase("./data/trx/nunit.trx"            , 3, 1)]
        [TestCase("./data/trx/nunit-datadriven.trx" , 5, 2)]
        [TestCase("./data/trx/nunit-ignore.trx"     , 5, 1)]
        [TestCase("./data/trx/nunit-with-stdout.trx", 1, 0)]
        [TestCase("./data/trx/nunit-memberdata.trx" , 5, 2)]
        [TestCase("./data/trx/xunit.trx"            , 3, 1)]
        [TestCase("./data/trx/xunit-datadriven.trx" , 3, 1)]
        [TestCase("./data/trx/xunit-ignore.trx"     , 4, 1)]
        [TestCase("./data/trx/xunit-memberdata.trx" , 5, 2)]
        [TestCase("./data/trx/nunit-testresultaggregation.trx", 3, 1)]
        public void File_given___correct_counts(string trxFile, int expectedTestCount, int expectedFailureCount)
        {
            XElement trx = XElement.Load(trxFile);
            var parser   = new TrxTestResultXmlParser(trx);

            parser.Parse();
            Models.TrxTest testData = parser.Result;

            var converter = new Trx2JunitTestConverter(testData);
            converter.Convert();

            Models.JUnitTest junitTest = converter.Result;
            var sut                    = new JUnitTestResultXmlBuilder(junitTest);
            sut.Build();

            XElement testsuite = sut.Result.Elements("testsuite").First();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedTestCount   , int.Parse(testsuite.Attribute("tests").Value)   , nameof(expectedTestCount));
                Assert.AreEqual(expectedFailureCount, int.Parse(testsuite.Attribute("failures").Value), nameof(expectedFailureCount));
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxUnitTestResult_with_stdout___system_out_set()
        {
            XElement trx = XElement.Load("./data/trx/nunit-with-stdout.trx");
            var parser   = new TrxTestResultXmlParser(trx);

            parser.Parse();
            Models.TrxTest testData = parser.Result;

            var converter = new Trx2JunitTestConverter(testData);
            converter.Convert();

            Models.JUnitTest junitTest = converter.Result;
            var sut                    = new JUnitTestResultXmlBuilder(junitTest);
            sut.Build();

            XElement testsuite = sut.Result.Elements("testsuite").First();
            XElement systemOut = testsuite.Element("system-out");

            Assert.IsNotNull(systemOut, nameof(systemOut));
            Assert.AreEqual("message written to stdout", systemOut.Value);
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxUnitTestResult_with_stdout___system_out_set_by_testcase()
        {
            XElement trx = XElement.Load("./data/trx/nunit-with-stdout.trx");
            var parser   = new TrxTestResultXmlParser(trx);

            parser.Parse();
            Models.TrxTest testData = parser.Result;

            var converter = new Trx2JunitTestConverter(testData);
            converter.Convert();

            Models.JUnitTest junitTest = converter.Result;
            var sut                    = new JUnitTestResultXmlBuilder(junitTest);
            sut.Build();

            XElement testsuite = sut.Result.Elements("testsuite").First();
            XElement testcase  = testsuite.Elements("testcase").First();
            XElement systemOut = testcase.Element("system-out");

            Assert.IsNotNull(systemOut, nameof(systemOut));
            Assert.AreEqual("message written to stdout", systemOut.Value);
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxUnitTestResult_with_stderr___system_err_set()
        {
            XElement trx = XElement.Load("./data/trx/nunit-with-stderr.trx");
            var parser   = new TrxTestResultXmlParser(trx);

            parser.Parse();
            Models.TrxTest testData = parser.Result;

            var converter = new Trx2JunitTestConverter(testData);
            converter.Convert();

            Models.JUnitTest junitTest = converter.Result;
            var sut                    = new JUnitTestResultXmlBuilder(junitTest);
            sut.Build();

            XElement testsuite = sut.Result.Elements("testsuite").First();
            XElement systemErr = testsuite.Element("system-err");

            Assert.IsNotNull(systemErr, nameof(systemErr));
            Assert.AreEqual("message written to stderr", systemErr.Value);
        }
        //---------------------------------------------------------------------
        [Test]
        public void TrxUnitTestResult_with_stderr___system_err_set_by_testcase()
        {
            XElement trx = XElement.Load("./data/trx/nunit-with-stderr.trx");
            var parser   = new TrxTestResultXmlParser(trx);

            parser.Parse();
            Models.TrxTest testData = parser.Result;

            var converter = new Trx2JunitTestConverter(testData);
            converter.Convert();

            Models.JUnitTest junitTest = converter.Result;
            var sut                    = new JUnitTestResultXmlBuilder(junitTest);
            sut.Build();

            XElement testsuite = sut.Result.Elements("testsuite").First();
            XElement testcase  = testsuite.Elements("testcase").First();
            XElement systemErr = testcase.Element("system-err");

            Assert.IsNotNull(systemErr, nameof(systemErr));
            Assert.AreEqual("message written to stderr", systemErr.Value);
        }
    }
}
