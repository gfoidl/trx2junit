using System;
using System.IO;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.FileSystemTests
{
    [TestFixture]
    public class CreateDirectory
    {
        [Test]
        public void Path_given___OK()
        {
            string path = $"./{Guid.NewGuid()}";
            var sut     = new FileSystem();

            sut.CreateDirectory(path);

            DirectoryAssert.Exists(path);

            try
            {
                Directory.Delete(path);
            }
            catch { }
        }
    }
}
