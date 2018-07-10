using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.TrxParserTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Parse_NUnit___OK()
        {
            Models.Test actual = this.ParseCore("./data/nunit.trx");

            Assert.IsNotNull(actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Parse_MsTest___OK()
        {
            Models.Test actual = this.ParseCore("./data/mstest.trx");

            Assert.IsNotNull(actual);
        }
        //---------------------------------------------------------------------
        private Models.Test ParseCore(string fileName)
        {
            XElement trx = XElement.Load(fileName);
            var sut      = new TrxParser(trx);

            sut.Parse();

            return sut.Result;
        }
    }
}
