using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitBuilderTests
{

    [TestFixture]
    public class Gh37
    {
        private readonly Models.Test _testData = new Models.Test();

        public Gh37()
        {
            var testGuid = Guid.NewGuid();
            var execGuids = Enumerable
                .Range(0, 4)
                .Select(x => Guid.NewGuid())
                .ToList();

            _testData.TestDefinitions.Add(testGuid,
                new TestDefinition
                {
                    Id = testGuid,
                    TestClass = "Class1",
                    TestMethod = "Method1",
                    ExecutionId = execGuids[0]
                });

            _testData.UnitTestResults[testGuid] = new List<UnitTestResult>();
            for (var i = 0; i < execGuids.Count; i++)
            {
                _testData.UnitTestResults[testGuid].Add(
                    new UnitTestResult
                    {
                        ExecutionId = execGuids[i],
                        TestId = testGuid,
                        TestName = "Method1",
                        Outcome = i != 0
                            ? Outcome.Passed
                            : Outcome.Failed,
                        Duration = new TimeSpan(0, 0, 1),
                        StartTime = DateTime.Now,
                        StackTrace = "",
                        Message = "",
                    });
            }
        }

        [Test]
        public void Builds___OK()
        {
            var sut = new JUnitBuilder(_testData);

            sut.Build();
        }

        [Test]
        public void Correct_Test_Suite()
        {
            var testsuite = this.GetTestSuite();

            Assert.AreEqual("Class1", testsuite.Attribute("name").Value);
        }

        [Test]
        public void Correct_Test_Suite_Test_Counts()
        {
            var testsuite = this.GetTestSuite();

            Assert.AreEqual(4, int.Parse(testsuite.Attribute("tests").Value));
        }

        [Test]
        public void Correct_Test_Suite_Failure_Counts()
        {
            var testsuite = this.GetTestSuite();

            Assert.AreEqual(1, int.Parse(testsuite.Attribute("failures").Value));
        }

        [Test]
        public void Correct_Test_Suite_Times()
        {
            var testsuite = this.GetTestSuite();

            Assert.AreEqual(4.0, int.Parse(testsuite.Attribute("time").Value));
        }

        private XElement GetTestSuite()
        {
            var builder = new JUnitBuilder(_testData);

            builder.Build();

            return builder.Result.Elements("testsuite").First();
        }
    }
}