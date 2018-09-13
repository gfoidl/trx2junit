using System;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerOptionsTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Single_file_given___OK()
        {
            string[] args = { "a.trx" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.IsNull(actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Multiple_files_given___OK()
        {
            string[] args = { "a.trx", "b.trx" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx", "b.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.IsNull(actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Single_file_and_output_path_at_end___OK()
        {
            string[] args = { "a.trx", "--output", "out" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Single_file_and_output_path_at_start___OK()
        {
            string[] args = { "--output", "out", "a.trx" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Multiple_files_and_output_path_at_end___OK()
        {
            string[] args = { "a.trx", "b.trx", "--output", "out" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx", "b.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Multiple_files_and_output_path_at_start___OK()
        {
            string[] args = { "--output", "out", "a.trx", "b.trx" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx", "b.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Multiple_files_and_output_path_in_middle___OK()
        {
            string[] args = { "a.trx", "--output", "out", "b.trx" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx", "b.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Multiple_output_directories___last_one_used()
        {
            string[] args = { "a.trx", "--output", "out", "b.trx", "--output", "junit-out" };

            var actual = WorkerOptions.Parse(args);

            Assert.Multiple(() =>
            {
                string[] expected = { "a.trx", "b.trx" };
                CollectionAssert.AreEqual(expected, actual.InputFiles);
                Assert.AreEqual("junit-out", actual.OutputDirectory);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Output_arg_given_but_no_value___throws_ArgumentException()
        {
            string[] args = { "a.trx", "--output" };

            ArgumentException actual = Assert.Throws<ArgumentException>(() => WorkerOptions.Parse(args));

            Assert.AreEqual("--output specified, but no value is given. An output-directory needs to specified in this case.", actual.Message);
        }
    }
}
