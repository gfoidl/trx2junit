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

            int? actual = xml.ReadInt("value");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.HasValue);
                Assert.AreEqual(expected, actual);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Xml_without_int_value___null()
        {
            var xml = new XElement("data", new XAttribute("foo", "abc"));

            int? actual = xml.ReadInt("foo");

            Assert.IsFalse(actual.HasValue);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Attribute_does_not_exists_on_xml___null()
        {
            var xml = new XElement("data");

            int? actual = xml.ReadInt("foo");

            Assert.IsFalse(actual.HasValue);
        }
    }
}
