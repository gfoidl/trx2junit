using System;
using System.Linq;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class TrxParser : ITrxParser
    {
        private static readonly XNamespace s_XN = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        //---------------------------------------------------------------------
        private readonly XElement _trx;
        private readonly Test     _test = new Test();
        //---------------------------------------------------------------------
        public Test Result => _test;
        //---------------------------------------------------------------------
        public TrxParser(XElement trx) => _trx = trx ?? throw new ArgumentNullException(nameof(trx));
        //---------------------------------------------------------------------
        public void Parse()
        {
            this.ReadTimes();
            this.ReadResults();
            this.ReadTestDefinitions();
            this.ReadUnitTestResults();
        }
        //---------------------------------------------------------------------
        private void ReadTimes()
        {
            XElement xTimes = _trx.Element(s_XN + "Times");

            _test.Times = new Times
            {
                Creation = xTimes.ReadDateTime("creation"),
                Queuing  = xTimes.ReadDateTime("queuing"),
                Start    = xTimes.ReadDateTime("start"),
                Finish   = xTimes.ReadDateTime("finish")
            };
        }
        //---------------------------------------------------------------------
        private void ReadResults()
        {
            XElement xResults  = _trx.Element(s_XN + "ResultSummary");
            XElement xCounters = xResults.Element(s_XN + "Counters");

            _test.ResultSummary = new ResultSummary
            {
                Outcome  = Enum.Parse<Outcome>(xResults.Attribute("outcome").Value),
                Error    = xCounters.ReadInt("error"),
                Executed = xCounters.ReadInt("executed"),
                Failed   = xCounters.ReadInt("failed"),
                Passed   = xCounters.ReadInt("passed"),
                Total    = xCounters.ReadInt("total")
            };

            // MsTest doesn't have this element, so be defensive
            _test.ResultSummary.StdOut = xResults
                .Elements(s_XN + "Output")
                .FirstOrDefault()
                ?.Elements(s_XN + "StdOut")
                ?.FirstOrDefault()
                ?.Value;
        }
        //---------------------------------------------------------------------
        private void ReadTestDefinitions()
        {
            XElement xTestDefinitions = _trx.Element(s_XN + "TestDefinitions");

            if (xTestDefinitions == null) return;

            foreach (XElement xUnitTest in xTestDefinitions.Elements())
            {
                var testDefinition = new TestDefinition
                {
                    Id          = xUnitTest.ReadGuid("id"),
                    ExecutionId = xUnitTest.Element(s_XN + "Execution") .ReadGuid("id"),
                    TestClass   = xUnitTest.Element(s_XN + "TestMethod").Attribute("className").Value,
                    TestMethod  = xUnitTest.Element(s_XN + "TestMethod").Attribute("name").Value
                };

                _test.TestDefinitions.Add(testDefinition.ExecutionId, testDefinition);
            }
        }
        //---------------------------------------------------------------------
        private void ReadUnitTestResults()
        {
            XElement xResults = _trx.Element(s_XN + "Results");

            if (xResults == null) return;

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

                XElement xOutput = xResult.Element(s_XN + "Output");
                if (xOutput != null)
                {
                    XElement xErrorInfo = xOutput.Element(s_XN + "ErrorInfo");
                    if (xErrorInfo != null)
                    {
                        XElement xMessage = xErrorInfo.Element(s_XN + "Message");
                        if (xMessage != null)
                            unitTestResult.Message = xMessage.Value;

                        XElement xStackTrace = xErrorInfo.Element(s_XN + "StackTrace");
                        if (xStackTrace != null)
                            unitTestResult.StackTrace = xStackTrace.Value;
                    }
                }

                _test.UnitTestResults.Add(unitTestResult.ExecutionId, unitTestResult);
            }
        }
    }
}
