// (c) gfoidl, all rights reserved

using System;
using System.Xml.Linq;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.XElementExtensionsTests;

[TestFixture]
public class WriteTrxDateTime
{
    [Test]
    public void Null_given___no_attribute_added()
    {
        var xmlExpected = new XElement("root");
        var xml         = new XElement("root");
        DateTime? value = null;

        xml.WriteTrxDateTime("dt", value);

        Assert.IsTrue(XElement.DeepEquals(xmlExpected, xml));
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Non_null_given___attribute_added()
    {
        DateTimeOffset? value = DateTimeOffset.Now;
        var xmlExpected       = new XElement("root", new XAttribute("dt", value.Value.ToTrxDateTime()));
        var xml               = new XElement("root");

        xml.WriteTrxDateTime("dt", value);

        Assert.IsTrue(XElement.DeepEquals(xmlExpected, xml));
    }
}
