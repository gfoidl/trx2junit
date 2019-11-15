using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.XElementExtensionsTests
{
    [TestFixture]
    public class ReadTimeSpan
    {
        [Test]
        public void Xml_with_valid_TimeSpan_given___OK()
        {
#if NETCOREAPP3_0
            TimeSpan expected = TimeSpan.FromSeconds(3 * 3600 + 2 * 60 + 1.0283517);
            var xml           = new XElement("data", new XAttribute("ts", "03:02:01.0283517"));
#else
            TimeSpan expected = TimeSpan.FromSeconds(3 * 3600 + 2 * 60 + 1.0280000);
            var xml           = new XElement("data", new XAttribute("ts", "03:02:01.0280000"));
#endif
            TestContext.WriteLine(xml);

            TimeSpan? actual = xml.ReadTimeSpan("ts");

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.HasValue);
                Assert.AreEqual(expected, actual.Value);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Xml_without_TimeSpan___null()
        {
            var xml = new XElement("data", new XAttribute("foo", "abc"));

            TimeSpan? actual = xml.ReadTimeSpan("foo");

            Assert.IsFalse(actual.HasValue);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Attribute_does_not_exists_on_xml___null()
        {
            var xml = new XElement("data");

            TimeSpan? actual = xml.ReadTimeSpan("foo");

            Assert.IsFalse(actual.HasValue);
        }
    }
}
