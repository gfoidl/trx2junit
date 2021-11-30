using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.XElementExtensionsTests
{
    [TestFixture]
    public class ReadGuid
    {
        [Test]
        public void Xml_with_valid_Guid_given___OK()
        {
            Guid expected = Guid.NewGuid();
            var xml       = new XElement("data", new XAttribute("id", expected.ToString()));

            Guid actual = xml.ReadGuid("id");

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Xml_without_Guid_value___empty_Guid()
        {
            var xml = new XElement("data", new XAttribute("foo", "abc"));

            Guid actual = xml.ReadGuid("foo");

            Assert.AreEqual(Guid.Empty, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Attribute_does_not_exists_on_xml___empty_Guid()
        {
            var xml = new XElement("data");

            Guid actual = xml.ReadGuid("foo");

            Assert.AreEqual(Guid.Empty, actual);
        }
    }
}
