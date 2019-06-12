﻿using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests.RunAsync
{
    [TestFixture]
    public class SameOutputLocation : Base
    {
        [Test]
        public async Task Single_file_given___converted()
        {
            string[] expectedFiles = { "./data/nunit.xml" };
            DeleteExpectedFiles(expectedFiles);

            Worker sut = this.CreateSut();

            string[] args = { "./data/nunit.trx" };
            var options   = new WorkerOptions(args);
            await sut.RunAsync(options);

            CheckExpectedFilesExist(expectedFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public async Task Multiple_files_given___converted()
        {
            string[] expectedFiles = { "./data/nunit.xml", "./data/mstest.xml", "./data/mstest-warning.xml" };
            DeleteExpectedFiles(expectedFiles);

            Worker sut = this.CreateSut();

            string[] args = { "./data/nunit.trx", "./data/mstest.trx", "./data/mstest-warning.trx" };
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
