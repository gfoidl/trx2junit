using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class Trx2JunitTestConverter : ITestConverter<TrxTest, JUnitTest>
    {
        private int                               _testId;
        private Counters                          _counters;
        private ILookup<Guid, TrxUnitTestResult>? _lookup;
        //---------------------------------------------------------------------
        public TrxTest SourceTest { get; }
        public JUnitTest Result   { get; } = new JUnitTest();
        //---------------------------------------------------------------------
        public Trx2JunitTestConverter(TrxTest? trxTest)
        {
            this.SourceTest = trxTest ?? throw new ArgumentNullException(nameof(trxTest));
        }
        //---------------------------------------------------------------------
        public void Convert()
        {
            var testSuites = this.SourceTest.TestDefinitions.GroupBy(t => t.TestClass);
            _lookup        = this.SourceTest.UnitTestResults.ToLookup(x => x.TestId);

            foreach (var testSuite in testSuites)
            {
                this.AddTestSuite(testSuite.Key, testSuite);
            }
        }
        //---------------------------------------------------------------------
        private void AddTestSuite(string testSuiteName, IEnumerable<TrxTestDefinition> trxTests)
        {
            this.ResetCounters();

            var testSuite = new JUnitTestSuite();
            this.Result.TestSuites.Add(testSuite);

            testSuite.Name = testSuiteName;
            testSuite.Id   = _testId++;

            foreach (TrxTestDefinition trxTest in trxTests)
            {
                this.AddTest(testSuite, trxTest);
            }

            testSuite.TestCount     = _counters.TestCount;
            testSuite.FailureCount  = _counters.Failures;
            testSuite.ErrorCount    = _counters.Errors;
            testSuite.SkippedCount  = _counters.Skipped;
            testSuite.TimeInSeconds = _counters.Time.TotalSeconds;

            if (_counters.TimeStamp.HasValue)
            {
                testSuite.TimeStamp = _counters.TimeStamp.Value;
            }

            Debug.Assert(testSuite.HostName != null);
        }
        //---------------------------------------------------------------------
        private void AddTest(JUnitTestSuite testSuite, TrxTestDefinition test)
        {
            Debug.Assert(_lookup != null);
            IEnumerable<TrxUnitTestResult> unitTestResults = _lookup![test.Id];

            foreach (TrxUnitTestResult unitTestResult in unitTestResults)
            {
                _counters.TestCount++;

                var testCase = new JUnitTestCase();
                testSuite.TestCases.Add(testCase);

                testSuite.HostName = unitTestResult.ComputerName;
                testCase.Name      = unitTestResult.TestName;
                testCase.ClassName = test.TestClass;

                if (!_counters.TimeStamp.HasValue)
                {
                    _counters.TimeStamp = unitTestResult.StartTime;
                }

                if (unitTestResult.Duration.HasValue)
                {
                    _counters.Time        += unitTestResult.Duration.Value;
                    testCase.TimeInSeconds = unitTestResult.Duration.Value.TotalSeconds;
                }

                if (unitTestResult.Outcome == TrxOutcome.NotExecuted)
                {
                    _counters.Skipped++;
                    testCase.Skipped = true;
                }
                else if (unitTestResult.Outcome == TrxOutcome.Failed)
                {
                    _counters.Failures++;

                    testCase.Error = new JUnitError
                    {
                        Message    = unitTestResult.Message ?? "",        // Message is allowed to be null
                        Type       = "not specified",
                        StackTrace = unitTestResult.StackTrace,
                    };
                }
            }
        }
        //---------------------------------------------------------------------
        private void ResetCounters() => _counters = default;
        //---------------------------------------------------------------------
        private struct Counters
        {
            public int       TestCount;
            public int       Failures;
            public int       Errors;
            public int       Skipped;
            public TimeSpan  Time;
            public DateTime? TimeStamp;
        }
    }
}
