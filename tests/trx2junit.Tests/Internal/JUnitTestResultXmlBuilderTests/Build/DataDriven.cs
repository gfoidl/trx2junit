using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitTestResultXmlBuilderTests.Build
{
    [TestFixture(Description = "https://github.com/gfoidl/trx2junit/issues/37")]
    public class DataDriven : Base
    {
        public DataDriven()
        {
            var testGuid      = Guid.NewGuid();
            var testExecGuids = Enumerable
                .Range(0, 4)
                .Select(_ => Guid.NewGuid())
                .ToList();

            _trxTest.TestDefinitions.Add(
                new TrxTestDefinition
                {
                    Id          = testGuid,
                    TestClass   = "Class1",
                    TestMethod  = "Method1",
                    ExecutionId = testExecGuids[0]
                });

            for (int i = 0; i < testExecGuids.Count; ++i)
            {
                _trxTest.UnitTestResults.Add(
                    new TrxUnitTestResult
                    {
                        ExecutionId  = testExecGuids[i],
                        TestId       = testGuid,
                        TestName     = "Method1",
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
            _junitTest     = converter.Result;
        }
        //---------------------------------------------------------------------
        [Test]
        public void Builds___OK()
        {
            var sut = new JUnitTestResultXmlBuilder(_junitTest);

            sut.Build();
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite()
        {
            XElement testsuite = this.GetTestSuite();

            Assert.AreEqual("Class1", testsuite.Attribute("name").Value);
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Test_Counts()
        {
            XElement testsuite = this.GetTestSuite();

            Assert.AreEqual(4, int.Parse(testsuite.Attribute("tests").Value));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Failure_Counts()
        {
            XElement testsuite = this.GetTestSuite();

            Assert.AreEqual(1, int.Parse(testsuite.Attribute("failures").Value));
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Times()
        {
            XElement testsuite = this.GetTestSuite();

            string actual = testsuite.Attribute("time").Value;
            TestContext.WriteLine(actual);

            Assert.AreEqual(4.0, decimal.Parse(actual, CultureInfo.InvariantCulture), "actual: {0}", actual);
        }
        //---------------------------------------------------------------------
        private XElement GetTestSuite() => this.GetTestSuites().First();
    }
}
