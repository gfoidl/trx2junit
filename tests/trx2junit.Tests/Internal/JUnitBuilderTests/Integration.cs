using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitBuilderTests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        [TestCase("./data/mstest-datadriven.trx", Ignore = "https://github.com/gfoidl/trx2junit/issues/43")]
        [TestCase("./data/nunit-datadriven.trx")]
        [TestCase("./data/xunit-datadriven.trx")]
        public void File_given___correct_test_count(string trxFile)
        {
            XElement trx = XElement.Load(trxFile);
            var parser   = new TrxParser(trx);

            parser.Parse();
            Models.Test testData = parser.Result;

            var sut = new JUnitBuilder(testData);
            sut.Build();
            XElement testsuite = sut.Result.Elements("testsuite").First();

            Assert.AreEqual(5, int.Parse(testsuite.Attribute("tests").Value));
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/mstest-datadriven.trx", Ignore = "https://github.com/gfoidl/trx2junit/issues/43")]
        [TestCase("./data/nunit-datadriven.trx")]
        [TestCase("./data/xunit-datadriven.trx")]
        public void File_given___correct_failure_count(string trxFile)
        {
            XElement trx = XElement.Load(trxFile);
            var parser   = new TrxParser(trx);

            parser.Parse();
            Models.Test testData = parser.Result;

            var sut = new JUnitBuilder(testData);
            sut.Build();
            XElement testsuite = sut.Result.Elements("testsuite").First();

            Assert.AreEqual(2, int.Parse(testsuite.Attribute("failures").Value));
        }
    }
}
