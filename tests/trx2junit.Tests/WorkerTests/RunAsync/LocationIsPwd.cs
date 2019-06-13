using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests.RunAsync
{
    [TestFixture]
    public class LocationIsPwd : Base
    {
        private string _curDir;
        //---------------------------------------------------------------------
        [SetUp]
        public void SetUp()
        {
            _curDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = "./data";
        }
        //---------------------------------------------------------------------
        [TearDown]
        public void TearDown()
        {
            Environment.CurrentDirectory = _curDir;
        }
        //---------------------------------------------------------------------
        [Test]
        public async Task Single_file_given___converted()
        {
            string[] expectedFiles = { "nunit.xml" };
            DeleteExpectedFiles(expectedFiles);

            Worker sut = this.CreateSut();

            string[] args = { "nunit.trx" };
            var options   = new WorkerOptions(args);
            await sut.RunAsync(options);

            CheckExpectedFilesExist(expectedFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public async Task Multiple_files_given___converted()
        {
            string[] expectedFiles = { "nunit.xml", "mstest.xml", "mstest-warning.xml" };
            DeleteExpectedFiles(expectedFiles);

            Worker sut = this.CreateSut();

            string[] args = { "nunit.trx", "mstest.trx", "mstest-warning.trx" };
            var options   = new WorkerOptions(args);
            await sut.RunAsync(options);

            CheckExpectedFilesExist(expectedFiles);
        }
        //---------------------------------------------------------------------
        private static void DeleteExpectedFiles(string[] files)
        {
            foreach (string file in files)
                File.Delete(file);
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
