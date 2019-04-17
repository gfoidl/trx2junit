using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitBuilderTests.Build
{
    [TestFixture]
    public class Default : Base
    {
        public Default()
        {
            AddTestResult("Class1", "Method1", Outcome.NotExecuted, new TimeSpan(0, 0,  1));
            AddTestResult("Class1", "Method2", Outcome.Failed     , new TimeSpan(0, 0,  3));
            AddTestResult("Class1", "Method3", Outcome.Failed     , new TimeSpan(0, 0,  2));
            AddTestResult("Class2", "Method1", Outcome.Passed     , new TimeSpan(0, 0,  3));
            AddTestResult("Class3", "Method1", Outcome.Failed     , new TimeSpan(0, 0,  8));
            AddTestResult("Class3", "Method2", Outcome.Passed     , new TimeSpan(0, 0, 12));
            AddTestResult("Class4", "Method1", Outcome.Completed  , new TimeSpan(0, 0,  1));
            AddTestResult("Class4", "Method2", Outcome.Warning    , new TimeSpan(0, 0,  1));
            //-----------------------------------------------------------------
            void AddTestResult(string testClass, string testMethod, Outcome testResult, TimeSpan? testDuration)
            {
                var testGuid     = Guid.NewGuid();
                var testExecGuid = Guid.NewGuid();

                _testData.TestDefinitions.Add(testGuid,
                    new TestDefinition
                    {
                        Id          = testGuid,
                        TestClass   = testClass,
                        TestMethod  = testMethod,
                        ExecutionId = testExecGuid,
                    });

                _testData.UnitTestResults.Add(testExecGuid,
                    new UnitTestResult
                    {
                        ExecutionId = testExecGuid,
                        TestId      = testGuid,
                        TestName    = testMethod,
                        Outcome     = testResult,
                        Duration    = testDuration,
                        StartTime   = DateTime.Now,
                        StackTrace  = "",
                        Message     = "",
                    });
            }
        }
        //---------------------------------------------------------------------
        [Test]
        public void Builds___OK()
        {
            var sut = new JUnitBuilder(_testData);

            sut.Build();
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suites()
        {
            List<XElement> testsuiteList = this.GetTestSuites();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("Class1", testsuiteList[0].Attribute("name").Value);
                Assert.AreEqual("Class2", testsuiteList[1].Attribute("name").Value);
                Assert.AreEqual("Class3", testsuiteList[2].Attribute("name").Value);
                Assert.AreEqual("Class4", testsuiteList[3].Attribute("name").Value);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Test_Counts()
        {
            List<XElement> testsuiteList = this.GetTestSuites();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, int.Parse(testsuiteList[0].Attribute("tests").Value));
                Assert.AreEqual(1, int.Parse(testsuiteList[1].Attribute("tests").Value));
                Assert.AreEqual(2, int.Parse(testsuiteList[2].Attribute("tests").Value));
                Assert.AreEqual(2, int.Parse(testsuiteList[3].Attribute("tests").Value));
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Failure_Counts()
        {
            List<XElement> testsuiteList = this.GetTestSuites();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, int.Parse(testsuiteList[0].Attribute("failures").Value));
                Assert.AreEqual(0, int.Parse(testsuiteList[1].Attribute("failures").Value));
                Assert.AreEqual(1, int.Parse(testsuiteList[2].Attribute("failures").Value));
                Assert.AreEqual(0, int.Parse(testsuiteList[3].Attribute("failures").Value));
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Error_Counts()
        {
            List<XElement> testsuiteList = this.GetTestSuites();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(0, int.Parse(testsuiteList[0].Attribute("errors").Value));
                Assert.AreEqual(0, int.Parse(testsuiteList[1].Attribute("errors").Value));
                Assert.AreEqual(0, int.Parse(testsuiteList[2].Attribute("errors").Value));
                Assert.AreEqual(0, int.Parse(testsuiteList[3].Attribute("errors").Value));
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Correct_Test_Suite_Times()
        {
            List<XElement> testsuiteList = this.GetTestSuites();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(6.0 , decimal.Parse(testsuiteList[0].Attribute("time").Value));
                Assert.AreEqual(3.0 , decimal.Parse(testsuiteList[1].Attribute("time").Value));
                Assert.AreEqual(20.0, decimal.Parse(testsuiteList[2].Attribute("time").Value));
                Assert.AreEqual(2.0 , decimal.Parse(testsuiteList[3].Attribute("time").Value));
            });
        }
    }
}
