// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Internal;
using gfoidl.Trx2Junit.Core.Models.Trx;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal.JUnitTestResultXmlBuilderTests.Build;

[TestFixture(Description = "https://github.com/gfoidl/trx2junit/issues/37")]
public class DataDriven : Base
{
    public DataDriven()
    {
        for (int i = 0; i < 4; ++i)
        {
            var testGuid     = Guid.NewGuid();
            var testExecGuid = Guid.NewGuid();

            _trxTest.TestDefinitions.Add(
                new TrxTestDefinition
                {
                    Id          = testGuid,
                    TestClass   = "SimpleUnitTest.Class1",
                    TestMethod  = $"Method1(arg: {i})",
                    ExecutionId = testExecGuid
                });

            _trxTest.UnitTestResults.Add(
                new TrxUnitTestResult
                {
                    ExecutionId  = testExecGuid,
                    TestId       = testGuid,
                    TestName     = $"Method1(arg: {i})",
                    Outcome      = i != 0 ? TrxOutcome.Passed : TrxOutcome.Failed,
                    Duration     = new TimeSpan(0, 0, 1),
                    StartTime    = DateTime.Now,
                    StackTrace   = "",
                    Message      = "",
                    ComputerName = Environment.MachineName
                });
        }

        var converter = new Trx2JunitTestConverter(_trxTest);
        converter.Convert();
        _junitTest    = converter.Result;
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Builds___OK()
    {
        var sut = new JUnitTestResultXmlBuilder(_junitTest);

        sut.Build();
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Correct_Test_Suite()
    {
        XElement testsuite = this.GetTestSuite();

        Assert.AreEqual("SimpleUnitTest.Class1", testsuite.Attribute("name").Value);
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Correct_Test_Suite_Name_and_ClassName()
    {
        XElement testsuite       = this.GetTestSuite();
        List<XElement> testcases = testsuite.Elements("testcase").ToList();

        Assert.Multiple(() =>
        {
            Assert.AreEqual("SimpleUnitTest.Class1", testcases[0].Attribute("classname").Value);
            Assert.AreEqual("SimpleUnitTest.Class1", testcases[1].Attribute("classname").Value);
            Assert.AreEqual("SimpleUnitTest.Class1", testcases[2].Attribute("classname").Value);
            Assert.AreEqual("SimpleUnitTest.Class1", testcases[3].Attribute("classname").Value);
        });

        Assert.Multiple(() =>
        {
            Assert.AreEqual("Method1(arg: 0)", testcases[0].Attribute("name").Value);
            Assert.AreEqual("Method1(arg: 1)", testcases[1].Attribute("name").Value);
            Assert.AreEqual("Method1(arg: 2)", testcases[2].Attribute("name").Value);
            Assert.AreEqual("Method1(arg: 3)", testcases[3].Attribute("name").Value);
        });
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Correct_Test_Suite_Test_Counts()
    {
        XElement testsuite = this.GetTestSuite();

        Assert.AreEqual(4, int.Parse(testsuite.Attribute("tests").Value));
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Correct_Test_Suite_Failure_Counts()
    {
        XElement testsuite = this.GetTestSuite();

        Assert.AreEqual(1, int.Parse(testsuite.Attribute("failures").Value));
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Correct_Test_Suite_Times()
    {
        XElement testsuite = this.GetTestSuite();

        string actual = testsuite.Attribute("time").Value;
        TestContext.WriteLine(actual);

        Assert.AreEqual(4.0, decimal.Parse(actual, CultureInfo.InvariantCulture), "actual: {0}", actual);
    }
    //-------------------------------------------------------------------------
    private XElement GetTestSuite() => this.GetTestSuites().First();
}
