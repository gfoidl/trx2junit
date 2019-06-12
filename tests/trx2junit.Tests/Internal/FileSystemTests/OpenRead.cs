using System.IO;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.FileSystemTests
{
    [TestFixture]
    public class OpenRead
    {
        [Test]
        public void Path_given___OK()
        {
            string path = "./data/nunit.trx";
            var sut     = new FileSystem();

            using (Stream actual = sut.OpenRead(path))
            {
                Assert.IsNotNull(actual);
            }
        }
    }
}
