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
        DateTime now = new DateTime(2019, 11, 10, 15, 33, 27);
        var xml      = new XElement("data", new XAttribute("dt", now.ToJUnitDateTime()));

        DateTime? actual = xml.ReadDateTime("dt");

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

        DateTime? actual = xml.ReadDateTime("foo");

        Assert.IsFalse(actual.HasValue);
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Attribute_does_not_exists_on_xml___null()
    {
        var xml = new XElement("data");

        DateTime? actual = xml.ReadDateTime("foo");

        Assert.IsFalse(actual.HasValue);
    }
}
