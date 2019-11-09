using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests
{
    [TestFixture]
    public class Convert
    {
        [Test]
        [TestCase("./data/trx/mstest.trx")]
        [TestCase("./data/trx/mstest-datadriven.trx")]
        [TestCase("./data/trx/mstest-ignore.trx")]
        [TestCase("./data/trx/mstest-warning.trx")]
        [TestCase("./data/trx/nunit.trx")]
        [TestCase("./data/trx/nunit-datadriven.trx")]
        [TestCase("./data/trx/nunit-ignore.trx")]
        [TestCase("./data/trx/nunit-no-tests.trx")]
        [TestCase("./data/trx/xunit.trx")]
        [TestCase("./data/trx/xunit-datadriven.trx")]
        [TestCase("./data/trx/xunit-ignore.trx")]
        [TestCase("./data/trx/xunit-memberdata.trx")]
        public async Task Trx_file_given___converted(string trxFile)
        {
            string junitFile = Path.ChangeExtension(trxFile, "xml");
            File.Delete(junitFile);
            var sut = new Worker();

            await sut.Convert(trxFile);

            bool actual = File.Exists(junitFile);

            Assert.IsTrue(actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/trx/mstest.trx")]
        [TestCase("./data/trx/mstest-datadriven.trx")]
        [TestCase("./data/trx/mstest-ignore.trx")]
        [TestCase("./data/trx/mstest-warning.trx")]
        [TestCase("./data/trx/nunit.trx")]
        [TestCase("./data/trx/nunit-datadriven.trx")]
        [TestCase("./data/trx/nunit-ignore.trx")]
        [TestCase("./data/trx/nunit-no-tests.trx")]
        [TestCase("./data/trx/xunit.trx")]
        [TestCase("./data/trx/xunit-datadriven.trx")]
        [TestCase("./data/trx/xunit-ignore.trx")]
        [TestCase("./data/trx/xunit-memberdata.trx")]
        public async Task Trx_file_given___generated_xml_is_valid_against_schema(string trxFile)
        {
            string junitFile = Path.ChangeExtension(trxFile, "xml");
            File.Delete(junitFile);
            var sut = new Worker();

            await sut.Convert(trxFile);

            ValidationHelper.IsXmlValidJunit(junitFile);
        }
    }
}
