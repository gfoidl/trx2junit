// (c) gfoidl, all rights reserved

using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Internal;
using gfoidl.Trx2Junit.Core.Models.JUnit;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.JUnitTestResultXmlParserTests;

[TestFixture]
public class Parse
{
    [Test]
    [TestCase("./data/junit/mstest.xml"           , 3, 1, 0)]
    [TestCase("./data/junit/mstest-datadriven.xml", 5, 2, 0)]
    [TestCase("./data/junit/mstest-ignore.xml"    , 4, 1, 0)]
    [TestCase("./data/junit/nunit.xml"            , 3, 1, 0)]
    [TestCase("./data/junit/nunit-datadriven.xml" , 5, 2, 0)]
    [TestCase("./data/junit/nunit-ignore.xml"     , 5, 1, 0)]
    [TestCase("./data/junit/nunit-memberdata.xml" , 5, 2, 0)]
    [TestCase("./data/junit/xunit.xml"            , 3, 1, 0)]
    [TestCase("./data/junit/xunit-datadriven.xml" , 3, 1, 0)]
    [TestCase("./data/junit/xunit-ignore.xml"     , 4, 1, 0)]
    [TestCase("./data/junit/xunit-memberdata.xml" , 5, 2, 0)]
    [TestCase("./data/junit/jenkins-style.xml"    , 3, 0, 1)]
    public void File_given___correct_counts(string junitFile, int expectedTestCount, int expectedFailureCount, int expectedErrorCount)
    {
        XElement trx = XElement.Load(junitFile);
        var sut      = new JUnitTestResultXmlParser(trx);

        sut.Parse();
        JUnitTest actual = sut.Result;

        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedTestCount   , actual.TestSuites[0].TestCount   , nameof(expectedTestCount));
            Assert.AreEqual(expectedFailureCount, actual.TestSuites[0].FailureCount, nameof(expectedFailureCount));
            Assert.AreEqual(expectedErrorCount  , actual.TestSuites[0].ErrorCount  , nameof(expectedErrorCount));
        });
    }
}
