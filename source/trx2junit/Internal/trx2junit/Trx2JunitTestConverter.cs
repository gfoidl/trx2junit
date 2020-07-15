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
        private ILookup<Guid, TrxUnitTestResult>? _trxTestDefinitionLookup;
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
            var testSuites           = this.SourceTest.TestDefinitions.GroupBy (t => t.TestClass);
            _trxTestDefinitionLookup = this.SourceTest.UnitTestResults.ToLookup(x => x.TestId);

            foreach (var testSuite in testSuites)
            {
                if (testSuite.Key is null) throw new InvalidOperationException("TestSuite.Key is null");

                this.AddTestSuite(testSuite.Key, testSuite);
            }
        }
        //---------------------------------------------------------------------
        private void AddTestSuite(string testSuiteName, IEnumerable<TrxTestDefinition> trxTestDefinitions)
        {
            this.ResetCounters();

            var testSuite = new JUnitTestSuite();
            this.Result.TestSuites.Add(testSuite);

            testSuite.Name = testSuiteName;
            testSuite.Id   = _testId++;

            foreach (TrxTestDefinition trxTest in trxTestDefinitions)
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
        }
        //---------------------------------------------------------------------
        private void AddTest(JUnitTestSuite junitTestSuite, TrxTestDefinition trxTestDefinition)
        {
            Debug.Assert(_trxTestDefinitionLookup != null);
            IEnumerable<TrxUnitTestResult> trxUnitTestResults = _trxTestDefinitionLookup![trxTestDefinition.Id];

            foreach (TrxUnitTestResult trxUnitTestResult in trxUnitTestResults)
            {
                _counters.TestCount++;

                var junitTestCase = new JUnitTestCase();
                junitTestSuite.TestCases.Add(junitTestCase);

                junitTestSuite.HostName = trxUnitTestResult.ComputerName;
                junitTestCase.Name      = trxUnitTestResult.TestName;
                junitTestCase.ClassName = trxTestDefinition.TestClass;

                if (!_counters.TimeStamp.HasValue)
                {
                    _counters.TimeStamp = trxUnitTestResult.StartTime;
                }

                if (trxUnitTestResult.Duration.HasValue)
                {
                    _counters.Time             += trxUnitTestResult.Duration.Value;
                    junitTestCase.TimeInSeconds = trxUnitTestResult.Duration.Value.TotalSeconds;
                }

                if (trxUnitTestResult.Outcome.IsSkipped())
                {
                    _counters.Skipped++;
                    junitTestCase.Skipped = true;
                }
                else if (trxUnitTestResult.Outcome.IsFailure())
                {
                    _counters.Failures++;

                    junitTestCase.Error = new JUnitError
                    {
                        Message    = trxUnitTestResult.Message ?? "",        // Message is allowed to be null
                        Type       = "not specified",
                        StackTrace = trxUnitTestResult.StackTrace,
                    };
                }

                junitTestCase.SystemErr = trxUnitTestResult.StdErr;
                junitTestCase.SystemOut = trxUnitTestResult.StdOut;
            }
        }
        //---------------------------------------------------------------------
        private void ResetCounters() => _counters = default;
        //---------------------------------------------------------------------
        private struct Counters
        {
            public int       TestCount;
            public int       Failures;
#pragma warning disable CS0649
            public int       Errors;
#pragma warning restore CS0649
            public int       Skipped;
            public TimeSpan  Time;
            public DateTime? TimeStamp;
        }
    }
}
