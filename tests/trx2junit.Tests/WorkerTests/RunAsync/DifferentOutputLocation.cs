using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests.RunAsync
{
    [TestFixture]
    public class DifferentOutputLocation : Base
    {
        [Test]
        public async Task Single_file_given___converted()
        {
            string[] expectedFiles = { "./data/out/nunit.xml" };

            if (Directory.Exists("./data/out"))
                Directory.Delete("./data/out", true);

            Worker sut = this.CreateSut();

            string[] args = { "./data/nunit.trx", "--output", "./data/out" };
            var options   = WorkerOptions.Parse(args);
            await sut.RunAsync(options);

            CheckExpectedFilesExist(expectedFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public async Task Multiple_files_given___converted()
        {
            string[] expectedFiles = { "./data/out/nunit.xml", "./data/out/mstest.xml", "./data/out/mstest-warning.xml" };

            if (Directory.Exists("./data/out"))
                Directory.Delete("./data/out", true);

            Worker sut = this.CreateSut();

            string[] args = { "./data/nunit.trx", "./data/mstest.trx", "./data/mstest-warning.trx", "--output", "./data/out" };
            var options   = WorkerOptions.Parse(args);
            await sut.RunAsync(options);

            CheckExpectedFilesExist(expectedFiles);
        }
        //---------------------------------------------------------------------
        private static void CheckExpectedFilesExist(string[] files)
        {
            Assert.Multiple(() =>
            {
                foreach (string file in files)
                {
                    bool actual = File.Exists(file);
                    Assert.IsTrue(actual, $"File '{file}' does not exist");
                }
            });
        }
    }
}
