﻿using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests
{
    [TestFixture, NonParallelizable]
    public class ConvertAsync
    {
        [OneTimeSetUp]
        public void OneTimeSetUp() => CleanFiles();
        //---------------------------------------------------------------------
        [OneTimeTearDown]
        public void OneTimeTearDown() => CleanFiles();
        //---------------------------------------------------------------------
        private static void CleanFiles()
        {
            DeleteFiles("./data/trx"  , "*.xml");
            DeleteFiles("./data/junit", "*.trx");
            //-----------------------------------------------------------------
            static void DeleteFiles(string path, string extension)
            {
                foreach (string file in Directory.EnumerateFiles(path, extension, SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/trx/mstest.trx")]
        [TestCase("./data/trx/mstest-datadriven.trx")]
        [TestCase("./data/trx/mstest-ignore.trx")]
        [TestCase("./data/trx/mstest-warning.trx")]
        [TestCase("./data/trx/nunit.trx")]
        [TestCase("./data/trx/nunit-datadriven.trx")]
        [TestCase("./data/trx/nunit-ignore.trx")]
        [TestCase("./data/trx/nunit-with-stdout.trx")]
        [TestCase("./data/trx/nunit-no-tests.trx")]
        [TestCase("./data/trx/xunit.trx")]
        [TestCase("./data/trx/xunit-datadriven.trx")]
        [TestCase("./data/trx/xunit-ignore.trx")]
        [TestCase("./data/trx/xunit-memberdata.trx")]
        public async Task Trx_file_given___converted(string trxFile)
        {
            Assume.That(() => ValidationHelper.IsXmlValidJunit(trxFile, validateJunit: false, TestContext.Out), Throws.Nothing);

            string junitFile = Path.ChangeExtension(trxFile, "xml");
            var sut          = new Worker();

            await sut.ConvertAsync(new Trx2JunitTestResultXmlConverter(), trxFile);

            bool actual = File.Exists(junitFile);
            Assert.IsTrue(actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/trx/mstest.trx")]
        [TestCase("./data/trx/mstest-datadriven.trx")]
        [TestCase("./data/trx/mstest-ignore.trx")]
        [TestCase("./data/trx/mstest-warning.trx")]
        [TestCase("./data/trx/nunit.trx")]
        [TestCase("./data/trx/nunit-datadriven.trx")]
        [TestCase("./data/trx/nunit-ignore.trx")]
        [TestCase("./data/trx/nunit-with-stdout.trx")]
        [TestCase("./data/trx/nunit-no-tests.trx")]
        [TestCase("./data/trx/xunit.trx")]
        [TestCase("./data/trx/xunit-datadriven.trx")]
        [TestCase("./data/trx/xunit-ignore.trx")]
        [TestCase("./data/trx/xunit-memberdata.trx")]
        public async Task Trx_file_given___generated_xml_is_valid_against_schema(string trxFile)
        {
            Assume.That(() => ValidationHelper.IsXmlValidJunit(trxFile, validateJunit: false, TestContext.Out), Throws.Nothing);

            string junitFile = Path.ChangeExtension(trxFile, "xml");
            var sut          = new Worker();

            await sut.ConvertAsync(new Trx2JunitTestResultXmlConverter(), trxFile);

            ValidationHelper.IsXmlValidJunit(junitFile, validateJunit: true, TestContext.Out);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/junit/mstest.xml")]
        [TestCase("./data/junit/mstest-datadriven.xml")]
        [TestCase("./data/junit/mstest-ignore.xml")]
        [TestCase("./data/junit/mstest-warning.xml")]
        [TestCase("./data/junit/nunit.xml")]
        [TestCase("./data/junit/nunit-datadriven.xml")]
        [TestCase("./data/junit/nunit-ignore.xml")]
        [TestCase("./data/junit/nunit-no-tests.xml")]
        [TestCase("./data/junit/xunit.xml")]
        [TestCase("./data/junit/xunit-datadriven.xml")]
        [TestCase("./data/junit/xunit-ignore.xml")]
        [TestCase("./data/junit/xunit-memberdata.xml")]
        [TestCase("./data/junit/jenkins-style.xml")]
        public async Task JUnit_file_given___converted(string junitFile)
        {
            Assume.That(() => ValidationHelper.IsXmlValidJunit(junitFile, validateJunit: true, TestContext.Out), Throws.Nothing);

            string trxFile = Path.ChangeExtension(junitFile, "trx");
            var sut        = new Worker();

            await sut.ConvertAsync(new Junit2TrxTestResultXmlConverter(), junitFile);

            bool actual = File.Exists(trxFile);
            Assert.IsTrue(actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/junit/mstest.xml")]
        [TestCase("./data/junit/mstest-datadriven.xml")]
        [TestCase("./data/junit/mstest-ignore.xml")]
        [TestCase("./data/junit/mstest-warning.xml")]
        [TestCase("./data/junit/nunit.xml")]
        [TestCase("./data/junit/nunit-datadriven.xml")]
        [TestCase("./data/junit/nunit-ignore.xml")]
        [TestCase("./data/junit/nunit-no-tests.xml")]
        [TestCase("./data/junit/xunit.xml")]
        [TestCase("./data/junit/xunit-datadriven.xml")]
        [TestCase("./data/junit/xunit-ignore.xml")]
        [TestCase("./data/junit/xunit-memberdata.xml")]
        [TestCase("./data/junit/jenkins-style.xml")]
        public async Task JUnit_file_given___generated_xml_is_valid_against_schema(string junitFile)
        {
            Assume.That(() => ValidationHelper.IsXmlValidJunit(junitFile, validateJunit: true, TestContext.Out), Throws.Nothing);

            string trxFile = Path.ChangeExtension(junitFile, "trx");
            var sut        = new Worker();

            await sut.ConvertAsync(new Junit2TrxTestResultXmlConverter(), junitFile);

            Assume.That(junitFile, Is.Not.EqualTo("./data/junit/nunit-no-tests.xml"), "not valid, VS will open it. Conversion so far OK");
            ValidationHelper.IsXmlValidJunit(trxFile, validateJunit: false, TestContext.Out);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/trx/jsingh-qualitrol.trx")]
        public async Task Trx_is_not_valid___logs_to_stderr_no_trx_file_and_exit_code_set_to_1(string trxFile)
        {
            string junitFile = Path.ChangeExtension(trxFile, "xml");
            var sut          = new Worker();

            TextWriter origConsoleErr = Console.Error;
            using var stringWriter    = new StringWriter();
            Console.SetError(stringWriter);

            await sut.ConvertAsync(new Trx2JunitTestResultXmlConverter(), trxFile);

            string consoleOutput = stringWriter.ToString();
            TestContext.WriteLine(consoleOutput);
            Console.SetError(origConsoleErr);

            Assert.Multiple(() =>
            {
                StringAssert.StartsWith("Given xml file is not a valid trx file", consoleOutput);
                Assert.AreEqual(1, Environment.ExitCode);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("./data/junit/no-junit.xml")]
        public async Task Junit_is_not_valid___logs_to_stderr_no_junit_file_and_exit_code_set_to_1(string junitFile)
        {
            string trxFile = Path.ChangeExtension(junitFile, "trx");
            var sut        = new Worker();

            TextWriter origConsoleErr = Console.Error;
            using var stringWriter    = new StringWriter();
            Console.SetError(stringWriter);

            await sut.ConvertAsync(new Junit2TrxTestResultXmlConverter(), junitFile);

            string consoleOutput = stringWriter.ToString();
            TestContext.WriteLine(consoleOutput);
            Console.SetError(origConsoleErr);

            Assert.Multiple(() =>
            {
                StringAssert.StartsWith("Given xml file is not a valid junit file", consoleOutput);
                Assert.AreEqual(1, Environment.ExitCode);
            });
        }
    }
}
