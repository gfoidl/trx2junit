﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.JUnitTestResultXmlBuilderTests.Build
{
    [TestFixture]
    public class Default : Base
    {
        public Default()
        {
            AddTestResult("Class1", "Method1", TrxOutcome.NotExecuted, new TimeSpan(0, 0,  1));
            AddTestResult("Class1", "Method2", TrxOutcome.Failed     , new TimeSpan(0, 0,  3));
            AddTestResult("Class1", "Method3", TrxOutcome.Failed     , new TimeSpan(0, 0,  2));
            AddTestResult("Class2", "Method1", TrxOutcome.Passed     , new TimeSpan(0, 0,  3));
            AddTestResult("Class3", "Method1", TrxOutcome.Failed     , new TimeSpan(0, 0,  8));
            AddTestResult("Class3", "Method2", TrxOutcome.Passed     , new TimeSpan(0, 0, 12));
            AddTestResult("Class4", "Method1", TrxOutcome.Completed  , new TimeSpan(0, 0,  1));
            AddTestResult("Class4", "Method2", TrxOutcome.Warning    , new TimeSpan(0, 0,  1));

            var converter = new Trx2JunitTestConverter(_trxTest);
            converter.Convert();
            _junitTest = converter.Result;
            //-----------------------------------------------------------------
            void AddTestResult(string testClass, string testMethod, TrxOutcome testResult, TimeSpan? testDuration)
            {
                var testGuid     = Guid.NewGuid();
                var testExecGuid = Guid.NewGuid();

                _trxTest.TestDefinitions.Add(
                    new TrxTestDefinition
                    {
                        Id          = testGuid,
                        TestClass   = testClass,
                        TestMethod  = testMethod,
                        ExecutionId = testExecGuid,
                    });

                _trxTest.UnitTestResults.Add(
                    new TrxUnitTestResult
                    {
                        ExecutionId  = testExecGuid,
                        TestId       = testGuid,
                        TestName     = testMethod,
                        Outcome      = testResult,
                        Duration     = testDuration,
                        StartTime    = DateTime.Now,
                        StackTrace   = "",
                        Message      = "",
                        ComputerName = Environment.MachineName
                    });
            }
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
                Assert.AreEqual(6.0 , decimal.Parse(testsuiteList[0].Attribute("time").Value, CultureInfo.InvariantCulture));
                Assert.AreEqual(3.0 , decimal.Parse(testsuiteList[1].Attribute("time").Value, CultureInfo.InvariantCulture));
                Assert.AreEqual(20.0, decimal.Parse(testsuiteList[2].Attribute("time").Value, CultureInfo.InvariantCulture));
                Assert.AreEqual(2.0 , decimal.Parse(testsuiteList[3].Attribute("time").Value, CultureInfo.InvariantCulture));
            });
        }
    }
}
