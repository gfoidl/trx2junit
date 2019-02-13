using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace trx2junit.Tests.Internal.JUnitBuilderTests
{
    [TestFixture]
    public class Build
    {
        private Models.Test testData = new Models.Test();

        public Build()
        {
            AddTestResult("Class1", "Method1", Outcome.NotExecuted, new TimeSpan(0, 0, 1));
            AddTestResult("Class1", "Method2", Outcome.Failed, new TimeSpan(0, 0, 3));
            AddTestResult("Class1", "Method3", Outcome.Failed, new TimeSpan(0, 0, 2));
            AddTestResult("Class2", "Method1", Outcome.Passed, new TimeSpan(0, 0, 3));
            AddTestResult("Class3", "Method1", Outcome.Failed, new TimeSpan(0, 0, 8));
            AddTestResult("Class3", "Method2", Outcome.Passed, new TimeSpan(0, 0, 12));
            AddTestResult("Class4", "Method1", Outcome.Completed, new TimeSpan(0, 0, 1));
            AddTestResult("Class4", "Method2", Outcome.Warning, new TimeSpan(0, 0, 1));

        }

        [Test]
        public void Build_Builds___OK()
        {
            RunJUnitBuilder(testData);
        }

        [Test]
        public void Build_Correct_Test_Suites()
        {
            var result = RunJUnitBuilder(testData);
            var testsuiteList = result.Elements("testsuite").ToList();

            Assert.AreEqual("Class1", testsuiteList[0].Attribute("name").Value);
            Assert.AreEqual("Class2", testsuiteList[1].Attribute("name").Value);
            Assert.AreEqual("Class3", testsuiteList[2].Attribute("name").Value);
            Assert.AreEqual("Class4", testsuiteList[3].Attribute("name").Value);
        }

        [Test]
        public void Build_Correct_Test_Suite_Test_Counts()
        {
            var result = RunJUnitBuilder(testData);
            var testsuiteList = result.Elements("testsuite").ToList();

            Assert.AreEqual(3, int.Parse(testsuiteList[0].Attribute("tests").Value));
            Assert.AreEqual(2, int.Parse(testsuiteList[1].Attribute("tests").Value));
            Assert.AreEqual(1, int.Parse(testsuiteList[2].Attribute("tests").Value));
            Assert.AreEqual(2, int.Parse(testsuiteList[3].Attribute("tests").Value));
        }

        [Test]
        public void Build_Correct_Test_Suite_Failure_Counts()
        {
            var result = RunJUnitBuilder(testData);
            var testsuiteList = result.Elements("testsuite").ToList();

            Assert.AreEqual(2, int.Parse(testsuiteList[0].Attribute("failures").Value));
            Assert.AreEqual(0, int.Parse(testsuiteList[1].Attribute("failures").Value));
            Assert.AreEqual(1, int.Parse(testsuiteList[2].Attribute("failures").Value));
            Assert.AreEqual(0, int.Parse(testsuiteList[3].Attribute("failures").Value));
        }

        [Test]
        public void Build_Correct_Test_Suite_Error_Counts()
        {
            var result = RunJUnitBuilder(testData);
            var testsuiteList = result.Elements("testsuite").ToList();

            Assert.AreEqual(0, int.Parse(testsuiteList[0].Attribute("errors").Value));
            Assert.AreEqual(0, int.Parse(testsuiteList[1].Attribute("errors").Value));
            Assert.AreEqual(0, int.Parse(testsuiteList[2].Attribute("errors").Value));
            Assert.AreEqual(0, int.Parse(testsuiteList[3].Attribute("errors").Value));
        }

        [Test]
        public void Build_Correct_Test_Suite_Times()
        {
            var result = RunJUnitBuilder(testData);
            var testsuiteList = result.Elements("testsuite").ToList();

            Assert.AreEqual(6.0, decimal.Parse(testsuiteList[0].Attribute("time").Value));
            Assert.AreEqual(3.0, decimal.Parse(testsuiteList[1].Attribute("time").Value));
            Assert.AreEqual(20.0, decimal.Parse(testsuiteList[2].Attribute("time").Value));
            Assert.AreEqual(2.0, decimal.Parse(testsuiteList[3].Attribute("time").Value));
        }

        private XElement RunJUnitBuilder(Models.Test test)
        {
            var builder = new JUnitBuilder(test);
            builder.Build();
            return builder.Result;
        }

        private void AddTestResult(string testClass, string testMethod, Outcome testResult, TimeSpan? testDuration)
        {
            var testGuid = Guid.NewGuid();
            var testExecGuid = Guid.NewGuid();
            testData.TestDefinitions.Add(testGuid,
                new TestDefinition
                {
                    Id = testGuid,
                    TestClass = testClass,
                    TestMethod = testMethod,
                    ExecutionId = testExecGuid,
                });
            testData.UnitTestResults.Add(testExecGuid,
                new UnitTestResult
                {
                    ExecutionId = testExecGuid,
                    TestId = testGuid,
                    Outcome = testResult,
                    Duration = testDuration,
                    StartTime = DateTime.Now,
                    StackTrace = "",
                    Message = "",
                });
        }
    }
}
