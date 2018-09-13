using System.IO;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests
{
    [TestFixture]
    public class GetJunitFile
    {
        [Test]
        public void No_outpath___only_extension_changed()
        {
            string trxFile  = "./data/nunit.trx";
            string expected = "./data/nunit.xml";

            string actual = Worker.GetJunitFile(trxFile);

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Outputpath_given___extension_changed_and_correct_path()
        {
            string trxFile  = "./data/nunit.trx";
            string expected = Path.Combine("./data/out", "nunit.xml");

            string actual = Worker.GetJunitFile(trxFile, "./data/out");

            Assert.AreEqual(expected, actual);
        }
    }
}
