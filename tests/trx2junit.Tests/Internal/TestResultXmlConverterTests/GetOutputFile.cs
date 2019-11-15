using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;
using trx2junit.Models;

namespace trx2junit.Tests.Internal.TestResultXmlConverterTests
{
    [TestFixture]
    public class GetOutputFile
    {
        [Test]
        public void No_outpath___only_extension_changed()
        {
            var sut         = new DummyTestResultXmlConverter();
            string trxFile  = "./data/nunit.trx";
            string expected = "./data/nunit.foo";

            string actual = sut.GetOutputFile(trxFile);

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Outputpath_given___extension_changed_and_correct_path()
        {
            var sut         = new DummyTestResultXmlConverter();
            string trxFile  = "./data/nunit.trx";
            string expected = Path.Combine("./data/out", "nunit.foo");

            string actual = sut.GetOutputFile(trxFile, "./data/out");

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        private class DummyTestResultXmlConverter : TestResultXmlConverter<TrxTest, JUnitTest>
        {
            protected override string Extension => "foo";
            //-----------------------------------------------------------------
            protected override Func<XElement, ITestResultXmlParser<TrxTest>> ParserFactory        => throw new NotImplementedException();
            protected override Func<TrxTest, ITestConverter<TrxTest, JUnitTest>> ConverterFactory => throw new NotImplementedException();
            protected override Func<JUnitTest, ITestResultXmlBuilder<JUnitTest>> BuilderFactory   => throw new NotImplementedException();
        }
    }
}
