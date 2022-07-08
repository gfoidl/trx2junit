// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.XElementExtensionsTests;

[TestFixture]
public class ReadDateTime
{
    [Test]
    public void Xml_with_valid_DateTime_given___OK()
    {
        DateTimeOffset now = new(2019, 11, 10, 15, 33, 27, TimeSpan.FromHours(1d));
        var xml            = new XElement("data", new XAttribute("dt", now.UtcDateTime.ToJUnitDateTime()));

        DateTimeOffset? actual = xml.ReadDateTime("dt");

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(now, actual.Value);
        });
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Xml_without_DateTime_value___null()
    {
        var xml = new XElement("data", new XAttribute("foo", "123"));

        DateTimeOffset? actual = xml.ReadDateTime("foo");

        Assert.IsFalse(actual.HasValue);
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Attribute_does_not_exists_on_xml___null()
    {
        var xml = new XElement("data");

        DateTimeOffset? actual = xml.ReadDateTime("foo");

        Assert.IsFalse(actual.HasValue);
    }
}
