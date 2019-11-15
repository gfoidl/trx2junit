using System.IO;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.FileSystemTests
{
    [TestFixture]
    public class EnumerateFiles
    {
        [Test]
        public void Path_and_pattern_given___OK()
        {
            string path    = "./data/trx";
            string pattern = "*.trx";
            var sut        = new FileSystem();

            var actual   = sut.EnumerateFiles(path, pattern);
            var expected = Directory.EnumerateFiles("./data/trx", "*.trx", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
