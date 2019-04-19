using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.TrxParserTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        [TestCase("./data/mstest.trx"           , 3, 3)]
        [TestCase("./data/mstest-datadriven.trx", 5, 3)]
        [TestCase("./data/nunit.trx"            , 3, 3)]
        [TestCase("./data/nunit-datadriven.trx" , 5, 5)]
        [TestCase("./data/nunit-memberdata.trx" , 5, 5)]
        [TestCase("./data/xunit.trx"            , 3, 3)]
        [TestCase("./data/xunit-datadriven.trx" , 5, 5)]
        [TestCase("./data/xunit-memberdata.trx" , 5, 3)]
        public void File_given___correct_counts(string trxFile, int expectedUnitTestResultsCount, int expectedTestDefinitionsCount)
        {
            XElement trx = XElement.Load(trxFile);
            var sut      = new TrxParser(trx);

            sut.Parse();
            Models.Test actual = sut.Result;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedUnitTestResultsCount, actual.UnitTestResults.Count, nameof(expectedUnitTestResultsCount));
                Assert.AreEqual(expectedTestDefinitionsCount, actual.TestDefinitions.Count, nameof(expectedTestDefinitionsCount));
            });
        }
    }
}
