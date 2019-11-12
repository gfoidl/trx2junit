using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitTestResultXmlParserTests
{
    [TestFixture]
    public class Parse
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
        [TestCase("./data/junit/xunit-datadriven.xml" , 3, 1)]
        [TestCase("./data/junit/xunit-ignore.xml"     , 4, 1)]
        [TestCase("./data/junit/xunit-memberdata.xml" , 5, 2)]
        public void File_given___correct_counts(string trxFile, int expectedTestCount, int expectedFailureCount)
        {
            XElement trx = XElement.Load(trxFile);
            var sut      = new JUnitTestResultXmlParser(trx);

            sut.Parse();
            Models.JUnitTest actual = sut.Result;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedTestCount   , actual.TestSuites[0].TestCount   , nameof(expectedTestCount));
                Assert.AreEqual(expectedFailureCount, actual.TestSuites[0].FailureCount, nameof(expectedFailureCount));
            });
        }
    }
}
