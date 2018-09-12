using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests
{
    [TestFixture]
    public class Convert
    {
        [Test]
        [TestCase("./data/nunit.trx")]
        [TestCase("./data/mstest.trx")]
        [TestCase("./data/mstest-warning.trx")]
        public async Task File_given___converted(string trxFile)
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
        [TestCase("./data/nunit.trx")]
        [TestCase("./data/mstest.trx")]
        [TestCase("./data/mstest-warning.trx")]
        public async Task File_given___generated_xml_is_valid_against_schema(string trxFile)
        {
            string junitFile = Path.ChangeExtension(trxFile, "xml");
            File.Delete(junitFile);
            var sut = new Worker();

            await sut.Convert(trxFile);

            ValidationHelper.IsXmlValidJunit(junitFile);
        }
    }
}
