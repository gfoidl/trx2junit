using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests
{
    [TestFixture]
    public class Convert
    {
        [Test]
        public async Task File_given___converted()
        {
            File.Delete("./data/nunit.xml");
            var sut = new Worker();

            await sut.Convert("./data/nunit.trx");

            bool actual = File.Exists("./data/nunit.xml");

            Assert.IsTrue(actual);
        }
    }
}
