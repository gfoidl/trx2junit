using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class TrxTestResultXmlBuilder : TrxBase, ITestResultXmlBuilder<TrxTest>
    {
        private static readonly Guid s_testTypeId = Guid.Parse("13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b");

        private readonly TrxTest  _test;
        private readonly XElement _xTrx       = new XElement(s_XN + "TestRun");
        private readonly Guid     _testListId = Guid.NewGuid();
        //---------------------------------------------------------------------
        public TrxTestResultXmlBuilder(TrxTest? test) => _test = test ?? throw new ArgumentNullException(nameof(test));
        //---------------------------------------------------------------------
        public TrxTest Test    => _test;
        public XElement Result => _xTrx;
        //---------------------------------------------------------------------
        public void Build()
        {
            Guid id = Guid.NewGuid();
            _xTrx.Add(new XAttribute("id", id));
            _xTrx.Add(new XAttribute("name", $"junit to trx, id: {id}"));

            this.WriteTimes();
            this.WriteResultSummary();
            this.WriteTestDefinitions();
            this.WriteUnitTestResults();
            this.WriteTestLists();
        }
        //---------------------------------------------------------------------
        private void WriteTimes()
        {
            var xTimes = new XElement(s_XN + "Times");
            _xTrx.Add(xTimes);

            TrxTimes? times = _test.Times;

            if (times != null)
            {
                xTimes.WriteTrxDateTime("creation", times.Creation);
                xTimes.WriteTrxDateTime("queuing" , times.Queuing);
                xTimes.WriteTrxDateTime("start"   , times.Start);
                xTimes.WriteTrxDateTime("finish"  , times.Finish);
            }
        }
        //---------------------------------------------------------------------
        private void WriteResultSummary()
        {
            var xResultSummary = new XElement(s_XN + "ResultSummary");
            _xTrx.Add(xResultSummary);

            var xCounters = new XElement(s_XN + "Counters");
            xResultSummary.Add(xCounters);

            TrxResultSummary? resultSummary = _test.ResultSummary;
            if (resultSummary != null)
            {
                xResultSummary.Add(new XAttribute("outcome", resultSummary.Outcome.ToString()));
                xCounters.Write("total"   , resultSummary.Total);
                xCounters.Write("passed"  , resultSummary.Passed);
                xCounters.Write("failed"  , resultSummary.Failed);
                xCounters.Write("executed", resultSummary.Executed);
                xCounters.Write("error"   , resultSummary.Errors);

                var xOutput = new XElement(s_XN + "Output");
                xResultSummary.Add(xOutput);

                xOutput.Add(new XElement(s_XN + "StdOut", resultSummary.StdOut));
            }
        }
        //---------------------------------------------------------------------
        private void WriteTestDefinitions()
        {
            var xTestDefinitions = new XElement(s_XN + "TestDefinitions");
            _xTrx.Add(xTestDefinitions);

            foreach (TrxTestDefinition trxTestDefinition in _test.TestDefinitions)
            {
                xTestDefinitions.Add(new XElement(s_XN + "UnitTest",
                    new XAttribute("id"  , trxTestDefinition.Id),
                    new XAttribute("name", trxTestDefinition.TestMethod),
                    new XElement(s_XN + "Execution", new XAttribute("id", trxTestDefinition.ExecutionId)),
                    new XElement(s_XN + "TestMethod",
                        new XAttribute("className", trxTestDefinition.TestClass),
                        new XAttribute("name"     , trxTestDefinition.TestMethod),
                        new XAttribute("codeBase" , "not available")
                    )
                ));
            }
        }
        //---------------------------------------------------------------------
        private void WriteUnitTestResults()
        {
            var xResults     = new XElement(s_XN + "Results");
            var xTestEntries = new XElement(s_XN + "TestEntries");

            _xTrx.Add(xResults);
            _xTrx.Add(xTestEntries);

            foreach (TrxUnitTestResult unitTestResult in _test.UnitTestResults)
            {
                var xUnitTestResult = new XElement(s_XN + "UnitTestResult");
                xResults.Add(xUnitTestResult);

                xUnitTestResult.Add(new XAttribute("executionId" , unitTestResult.ExecutionId));
                xUnitTestResult.Add(new XAttribute("testId"      , unitTestResult.TestId));
                xUnitTestResult.Add(new XAttribute("testName"    , unitTestResult.TestName));
                xUnitTestResult.Add(new XAttribute("outcome"     , unitTestResult.Outcome));
                xUnitTestResult.Add(new XAttribute("computerName", unitTestResult.ComputerName));
                xUnitTestResult.Add(new XAttribute("testType"    , s_testTypeId));
                xUnitTestResult.Add(new XAttribute("testListId"  , _testListId));
                xUnitTestResult.WriteTrxDateTime("startTime"     , unitTestResult.StartTime);
                xUnitTestResult.WriteTrxDateTime("endTime"       , unitTestResult.EndTime);
                xUnitTestResult.Write("duration"                 , unitTestResult.Duration);

                XElement? xOutput = null;

                if (unitTestResult.Message != null || unitTestResult.StackTrace != null)
                {
                    xOutput = new XElement(s_XN + "Output");

                    var xErrorInfo = new XElement(s_XN + "ErrorInfo");
                    xOutput.Add(xErrorInfo);

                    if (unitTestResult.Message != null)
                    {
                        xErrorInfo.Add(new XElement(s_XN + "Message", unitTestResult.Message));
                    }

                    if (unitTestResult.StackTrace != null)
                    {
                        xErrorInfo.Add(new XElement(s_XN + "StackTrace", unitTestResult.StackTrace));
                    }
                }

                if (unitTestResult.StdOut != null)
                {
                    xOutput ??= new XElement(s_XN + "Output");
                    xOutput.Add(new XElement(s_XN + "StdOut", unitTestResult.StdOut));
                }

                if (unitTestResult.StdErr != null)
                {
                    xOutput ??= new XElement(s_XN + "Output");
                    xOutput.Add(new XElement(s_XN + "StdErr", unitTestResult.StdErr));
                }

                if (xOutput != null)
                {
                    xUnitTestResult.Add(xOutput);
                }

                xTestEntries.Add(new XElement(s_XN + "TestEntry",
                    new XAttribute("testId"     , unitTestResult.TestId),
                    new XAttribute("executionId", unitTestResult.ExecutionId),
                    new XAttribute("testListId" , _testListId)
                ));
            }
        }
        //---------------------------------------------------------------------
        private void WriteTestLists()
        {
            _xTrx.Add(new XElement(s_XN + "TestLists",
                new XElement(s_XN + "TestList",
                    new XAttribute("name", "Results Not in a List"),
                    new XAttribute("id", _testListId)
                )
            ));
        }
    }
}
