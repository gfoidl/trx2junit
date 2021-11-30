// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.XElementExtensionsTests;

[TestFixture]
public class Write
{
    [Test]
    public void Null_given___no_attribute_added()
    {
        var xmlExpected = new XElement("root");
        var xml         = new XElement("root");
        int? value      = null;

        xml.Write("id", value);

        Assert.IsTrue(XElement.DeepEquals(xmlExpected, xml));
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Non_null_given___attribute_added()
    {
        var xmlExpected = new XElement("root", new XAttribute("id", 42));
        var xml         = new XElement("root");
        int? value      = 42;

        xml.Write("id", value);

        Assert.IsTrue(XElement.DeepEquals(xmlExpected, xml));
    }
    //-------------------------------------------------------------------------
    [Test]
    public void DateTime_as_type___throws_InvalidOperation()
    {
        var xml         = new XElement("root");
        DateTime? value = null;

        InvalidOperationException actual = Assert.Throws<InvalidOperationException>(() => xml.Write("dt", value));

        Assert.AreEqual("Use WriteTrxDateTime method instead", actual.Message);
    }
}
