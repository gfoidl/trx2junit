using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace trx2junit
{
    static class Program
    {
        private static readonly XNamespace XN = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private static Times                            _times;
        private static ResultSummary                    _results;
        private static Dictionary<Guid, TestDefinition> _testDefinitions;
        private static Dictionary<Guid, UnitTestResult> _unitTestResults;
        //---------------------------------------------------------------------
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("first arg must be the trx-file");
                Environment.Exit(1);
            }

            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            string trxFile   = args[0];
            string jUnitFile = Path.ChangeExtension(trxFile, "xml");
            ReadTrx(trxFile);
            WriteJUnit(jUnitFile);

            Console.WriteLine("\nbye.");
            if (Debugger.IsAttached)
                Console.ReadKey();
        }
        //---------------------------------------------------------------------
        private static void ReadTrx(string trxFileName)
        {
            XElement trx = XElement.Load(trxFileName);
            ReadTimes(trx);
            ReadResults(trx);
            ReadTestDefinitions(trx);
            ReadUnitTestResults(trx);
        }
        //---------------------------------------------------------------------
        private static void ReadTimes(XElement trx)
        {
            XElement xTimes = trx.Element(XN + "Times");

            _times = new Times
            {
                Creation = xTimes.ReadDateTime("creation"),
                Queuing  = xTimes.ReadDateTime("queuing"),
                Start    = xTimes.ReadDateTime("start"),
                Finish   = xTimes.ReadDateTime("finish")
            };
        }
        //---------------------------------------------------------------------
        private static void ReadResults(XElement trx)
        {
            XElement xResults  = trx.Element(XN + "ResultSummary");
            XElement xCounters = xResults.Element(XN + "Counters");

            _results = new ResultSummary
            {
                StdOut   = xResults.Element(XN + "Output").Element(XN + "StdOut").Value,
                Outcome  = Enum.Parse<Outcome>(xResults.Attribute("outcome").Value),
                Error    = xCounters.ReadInt("error"),
                Executed = xCounters.ReadInt("executed"),
                Failed   = xCounters.ReadInt("failed"),
                Passed   = xCounters.ReadInt("passed"),
                Total    = xCounters.ReadInt("total")
            };
        }
        //---------------------------------------------------------------------
        private static void ReadTestDefinitions(XElement trx)
        {
            XElement xTestDefinitions = trx.Element(XN + "TestDefinitions");

            _testDefinitions = new Dictionary<Guid, TestDefinition>();

            foreach (XElement xUnitTest in xTestDefinitions.Elements())
            {
                var testDefinition = new TestDefinition
                {
                    Id          = xUnitTest.ReadGuid("id"),
                    ExecutionId = xUnitTest.Element(XN + "Execution").ReadGuid("id"),
                    TestClass   = xUnitTest.Element(XN + "TestMethod").Attribute("className").Value,
                    TestMethod  = xUnitTest.Element(XN + "TestMethod").Attribute("name").Value
                };

                _testDefinitions.Add(testDefinition.ExecutionId, testDefinition);
            }
        }
        //---------------------------------------------------------------------
        private static void ReadUnitTestResults(XElement trx)
        {
            XElement xResults = trx.Element(XN + "Results");

            _unitTestResults = new Dictionary<Guid, UnitTestResult>();

            foreach (XElement xResult in xResults.Elements())
            {
                var unitTestResult = new UnitTestResult
                {
                    Duration    = xResult.ReadTimeSpan("duration"),
                    EndTime     = xResult.ReadDateTime("endTime"),
                    ExecutionId = xResult.ReadGuid("executionId"),
                    Outcome     = Enum.Parse<Outcome>(xResult.Attribute("outcome").Value),
                    StartTime   = xResult.ReadDateTime("startTime"),
                    TestId      = xResult.ReadGuid("testId"),
                    TestName    = xResult.Attribute("testName").Value
                };

                XElement xOutput = xResult.Element(XN + "Output");
                if (xOutput != null)
                {
                    XElement xErrorInfo = xOutput.Element(XN + "ErrorInfo");
                    if (xErrorInfo != null)
                    {
                        XElement xMessage = xErrorInfo.Element(XN + "Message");
                        if (xMessage != null)
                            unitTestResult.Message = xMessage.Value;

                        XElement xStackTrace = xErrorInfo.Element(XN + "StackTrace");
                        if (xStackTrace != null)
                            unitTestResult.StackTrace = xStackTrace.Value;
                    }
                }

                _unitTestResults.Add(unitTestResult.ExecutionId, unitTestResult);
            }
        }
        //---------------------------------------------------------------------
        private static void WriteJUnit(string jUnitFile)
        {
            var xJUnit     = new XElement("testsuites");
            var testSuites = _testDefinitions.GroupBy(t => t.Value.TestClass);
            int testId     = default;

            foreach (var testSuite in testSuites)
            {
                string testClass = testSuite.Key;
                var xTestSuite = new XElement("testsuite");
                xTestSuite.Add(new XAttribute("name", testClass));
                xTestSuite.Add(new XAttribute("hostname", Environment.MachineName));
                xTestSuite.Add(new XAttribute("package", ".net Core"));
                xTestSuite.Add(new XAttribute("id", testId++));
                xTestSuite.Add(new XElement("properties"));

                int errors          = default;
                int tests           = default;
                int failures        = default;
                TimeSpan time       = default;
                DateTime? timestamp = default;

                foreach (var test in testSuite)
                {
                    tests++;

                    var xTestCase = new XElement("testcase");
                    xTestSuite.Add(xTestCase);

                    xTestCase.Add(new XAttribute("classname", test.Value.TestClass));
                    xTestCase.Add(new XAttribute("name", test.Value.TestMethod));

                    Guid executionId = test.Value.ExecutionId;
                    UnitTestResult unitTestResult = _unitTestResults[executionId];

                    if (!timestamp.HasValue) timestamp = unitTestResult.StartTime;

                    time += unitTestResult.Duration;
                    xTestCase.Add(new XAttribute("time", (decimal)unitTestResult.Duration.TotalSeconds));

                    if (unitTestResult.Outcome == Outcome.NotExecuted)
                        xTestCase.Add(new XElement("skipped"));
                    else if (unitTestResult.Outcome == Outcome.Failed)
                    {
                        failures++;
                        xTestCase.Add(new XElement("failure",
                            unitTestResult.StackTrace,
                            new XAttribute("message", unitTestResult.Message),
                            new XAttribute("type", "error")
                            )
                        );
                    }
                }

                xTestSuite.Add(new XAttribute("errors", errors));
                xTestSuite.Add(new XAttribute("tests", tests));
                xTestSuite.Add(new XAttribute("failures", failures));
                xTestSuite.Add(new XAttribute("time", (decimal)time.TotalSeconds));
                xTestSuite.Add(new XAttribute("timestamp", timestamp.Value.ToJUnitDateTime()));
                xTestSuite.Add(new XElement("system-out"));
                xTestSuite.Add(new XElement("system-err"));

                xJUnit.Add(xTestSuite);
            }

            var utf8 = new UTF8Encoding(false);
            using (var sw = new StreamWriter(jUnitFile, false, utf8))
                xJUnit.Save(sw);
        }
    }
}