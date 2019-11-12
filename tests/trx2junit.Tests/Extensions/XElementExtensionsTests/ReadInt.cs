using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.XElementExtensionsTests
{
    [TestFixture]
    public class ReadInt
    {
        [Test]
        public void Xml_with_valid_int_given___OK()
        {
            int expected = new Random(42).Next();
            var xml      = new XElement("data", new XAttribute("value", expected));

            int actual = xml.ReadInt("value");

            Assert.AreEqual(expected, actual);
        }
    }
}
