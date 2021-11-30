using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using trx2junit.Models;

namespace trx2junit
{
    public class JUnit2TrxTestConverter : ITestConverter<JUnitTest, TrxTest>
    {
        public JUnitTest SourceTest { get; }
        public TrxTest Result       { get; } = new TrxTest();
        //---------------------------------------------------------------------
        public JUnit2TrxTestConverter(JUnitTest junitTest)
        {
            this.SourceTest = junitTest ?? throw new ArgumentNullException(nameof(junitTest));
        }
        //---------------------------------------------------------------------
        public void Convert()
        {
            this.BuildResults(this.SourceTest.TestSuites);
        }
        //---------------------------------------------------------------------
        private void BuildResults(ICollection<JUnitTestSuite> jUnitTestSuites)
        {
            var trxTimes         = new TrxTimes();
            var trxResultSummary = new TrxResultSummary();

            var resultSummaryStdOutStringBuilder = new StringBuilder();
            TimeSpan executionTime               = default;

            foreach (JUnitTestSuite jUnitTestSuite in jUnitTestSuites)
            {
                trxResultSummary.Total    += jUnitTestSuite.TestCount;
                trxResultSummary.Errors   += jUnitTestSuite.ErrorCount                              ?? 0;
                trxResultSummary.Executed += jUnitTestSuite.TestCount - jUnitTestSuite.SkippedCount ?? 0;
                trxResultSummary.Failed   += jUnitTestSuite.FailureCount                            ?? 0;
                trxResultSummary.Passed   += (jUnitTestSuite.TestCount - (jUnitTestSuite.ErrorCount ?? 0) - (jUnitTestSuite.FailureCount ?? 0) - (jUnitTestSuite.SkippedCount ?? 0));
                executionTime             += TimeSpan.FromSeconds(jUnitTestSuite.TimeInSeconds);
                SetOutcome          (jUnitTestSuite, trxResultSummary);
                SetCreationTimestamp(jUnitTestSuite, trxTimes);
                SetOutput           (jUnitTestSuite, resultSummaryStdOutStringBuilder);

                foreach (JUnitTestCase junitTestCase in jUnitTestSuite.TestCases)
                {
                    this.ProcessTestCases(jUnitTestSuite, junitTestCase);
                }
            }

            if (resultSummaryStdOutStringBuilder.Length > 0)
            {
                trxResultSummary.StdOut = resultSummaryStdOutStringBuilder.ToString();
            }

            if (this.Result.TestDefinitions.Count > 0)
            {
                Debug.Assert(trxTimes.Creation.HasValue);
                trxTimes.Finish = trxTimes.Creation!.Value + executionTime;

                this.Result.Times         = trxTimes;
                this.Result.ResultSummary = trxResultSummary;
            }
        }
        //---------------------------------------------------------------------
        private void ProcessTestCases(JUnitTestSuite jUnitTestSuite, JUnitTestCase jUnitTestCase)
        {
            Guid executionId = Guid.NewGuid();
            Guid testId      = Guid.NewGuid();

            var trxUnitTestResult = new TrxUnitTestResult
            {
                ExecutionId  = executionId,
                TestId       = testId,
                StartTime    = jUnitTestSuite.TimeStamp,
                Duration     = TimeSpan.FromSeconds(jUnitTestCase.TimeInSeconds),
                EndTime      = jUnitTestSuite.TimeStamp.AddSeconds(jUnitTestCase.TimeInSeconds),
                TestName     = jUnitTestCase.Name,
                Outcome      = TrxOutcome.Passed,
                ComputerName = jUnitTestSuite.HostName
            };

            if (jUnitTestCase.Skipped)
            {
                trxUnitTestResult.Outcome = TrxOutcome.NotExecuted;
            }

            if (jUnitTestCase.Error != null)
            {
                trxUnitTestResult.Outcome    = TrxOutcome.Failed;
                trxUnitTestResult.Message    = jUnitTestCase.Error.Message;
                trxUnitTestResult.StackTrace = jUnitTestCase.Error.StackTrace;
            }

            trxUnitTestResult.StdErr = jUnitTestCase.SystemErr;
            trxUnitTestResult.StdOut = jUnitTestCase.SystemOut;

            var trxTestDefinition = new TrxTestDefinition
            {
                Id          = testId,
                ExecutionId = executionId,
                TestClass   = jUnitTestCase.ClassName,
                TestMethod  = jUnitTestCase.Name
            };

            this.Result.UnitTestResults.Add(trxUnitTestResult);
            this.Result.TestDefinitions.Add(trxTestDefinition);
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal static TrxOutcome GetOutcome(JUnitTestSuite jUnitTestSuite)
        {
            if (jUnitTestSuite.FailureCount > 0 || jUnitTestSuite.ErrorCount > 0)
                return TrxOutcome.Failed;

            if (jUnitTestSuite.SkippedCount > 0)
                return TrxOutcome.NotExecuted;

            return TrxOutcome.Passed;
        }
        //---------------------------------------------------------------------
        private static void SetOutcome(JUnitTestSuite jUnitTestSuite, TrxResultSummary trxResultSummary)
        {
            TrxOutcome outcome = GetOutcome(jUnitTestSuite);

            if (outcome != TrxOutcome.Passed)
            {
                trxResultSummary.Outcome = outcome;
            }
        }
        //---------------------------------------------------------------------
        // internal for testing
        internal static void SetCreationTimestamp(JUnitTestSuite jUnitTestSuite, TrxTimes trxTimes)
        {
            if (trxTimes.Creation.HasValue)
            {
                if (jUnitTestSuite.TimeStamp < trxTimes.Creation)
                {
                    trxTimes.Creation = jUnitTestSuite.TimeStamp;
                }
            }
            else
            {
                trxTimes.Creation = jUnitTestSuite.TimeStamp;
            }
        }
        //---------------------------------------------------------------------
        private static void SetOutput(JUnitTestSuite jUnitTestSuite, StringBuilder resultSummaryStdOutStringBuilder)
        {
            if (!string.IsNullOrWhiteSpace(jUnitTestSuite.SystemOut))
            {
                resultSummaryStdOutStringBuilder.AppendLine(jUnitTestSuite.SystemOut);
            }

            if (!string.IsNullOrWhiteSpace(jUnitTestSuite.SystemErr))
            {
                resultSummaryStdOutStringBuilder.AppendLine(jUnitTestSuite.SystemErr);
            }
        }
    }
}
