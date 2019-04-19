using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitBuilderTests
{
    [TestFixture]
    public class Integration
    {
        [Test]
        [TestCase("./data/mstest.trx"           , 3, 1)]
        [TestCase("./data/mstest-datadriven.trx", 5, 2)]
        [TestCase("./data/mstest-ignore.trx"    , 4, 1)]
        [TestCase("./data/nunit.trx"            , 3, 1)]
        [TestCase("./data/nunit-datadriven.trx" , 5, 2)]
        [TestCase("./data/nunit-ignore.trx"     , 5, 1)]
        [TestCase("./data/nunit-memberdata.trx" , 5, 2)]
        [TestCase("./data/xunit.trx"            , 3, 1)]
        [TestCase("./data/xunit-datadriven.trx" , 3, 1)]
        [TestCase("./data/xunit-ignore.trx"     , 4, 1)]
        [TestCase("./data/xunit-memberdata.trx" , 5, 2)]
        public void File_given___correct_counts(string trxFile, int expectedTestCount, int expectedFailureCount)
        {
            XElement trx = XElement.Load(trxFile);
            var parser   = new TrxParser(trx);

            parser.Parse();
            Models.Test testData = parser.Result;

            var sut = new JUnitBuilder(testData);
            sut.Build();
            XElement testsuite = sut.Result.Elements("testsuite").First();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedTestCount   , int.Parse(testsuite.Attribute("tests").Value)   , nameof(expectedTestCount));
                Assert.AreEqual(expectedFailureCount, int.Parse(testsuite.Attribute("failures").Value), nameof(expectedFailureCount));
            });
        }
    }
}
