// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.XElementExtensionsTests;

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
    //-------------------------------------------------------------------------
    [Test]
    public void Xml_without_double_value___throws_Exception()
    {
        var xml = new XElement("data", new XAttribute("foo", "abc"));

        var ex = Assert.Throws<Exception>(() => xml.ReadDouble("foo"));

        string expectedMsg = "The required attribute 'foo' does not exists";
        Assert.AreEqual(expectedMsg, ex.Message);
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Attribute_does_not_exists_on_xml___throws_Exception()
    {
        var xml = new XElement("data");

        var ex = Assert.Throws<Exception>(() => xml.ReadDouble("foo"));

        string expectedMsg = "The required attribute 'foo' does not exists";
        Assert.AreEqual(expectedMsg, ex.Message);
    }
}
