using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.XElementExtensionsTests
{
    [TestFixture]
    public class ReadDouble
    {
        [Test]
        public void Xml_with_valid_double_given___OK()
        {
            double expected = new Random(42).NextDouble();
            var xml         = new XElement("data", new XAttribute("value", expected));

            double actual = xml.ReadDouble("value");

            Assert.AreEqual(expected, actual);
        }
    }
}
