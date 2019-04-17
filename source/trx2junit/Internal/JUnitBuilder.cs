using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class JUnitBuilder : IJUnitBuilder
    {
        private readonly Test     _test;
        private readonly XElement _xJUnit = new XElement("testsuites");
        private int               _testId;
        private int               _errors;
        private int               _testCount;
        private int               _failures;
        private TimeSpan          _time;
        private DateTime?         _timeStamp;
        //---------------------------------------------------------------------
        private ILookup<Guid, UnitTestResult> _lookup;
        //---------------------------------------------------------------------
        public XElement Result => _xJUnit;
        //---------------------------------------------------------------------
        public JUnitBuilder(Test test) => _test = test ?? throw new ArgumentNullException(nameof(test));
        //---------------------------------------------------------------------
        public void Build()
        {
            var testSuites = _test.TestDefinitions.GroupBy(t => t.TestClass);

            _lookup = _test.UnitTestResults.ToLookup(x => x.TestId);

            foreach (var testSuite in testSuites)
                this.AddTestSuite(testSuite.Key, testSuite);
        }
        //---------------------------------------------------------------------
        private void AddTestSuite(string testSuiteName, IEnumerable<TestDefinition> tests)
        {
            this.ResetCounters();

            var xTestSuite = new XElement("testsuite");

            xTestSuite.Add(new XAttribute("name"    , testSuiteName));
            xTestSuite.Add(new XAttribute("hostname", Environment.MachineName));
            xTestSuite.Add(new XAttribute("package" , ".net Core"));
            xTestSuite.Add(new XAttribute("id"      , _testId++));

            xTestSuite.Add(new XElement("properties"));
            foreach (var test in tests)
                this.AddTest(xTestSuite, test);

            xTestSuite.Add(new XAttribute("errors"  , _errors));
            xTestSuite.Add(new XAttribute("tests"   , _testCount));
            xTestSuite.Add(new XAttribute("failures", _failures));
            xTestSuite.Add(new XAttribute("time"    , (decimal)_time.TotalSeconds));
            xTestSuite.Add(new XElement("system-out"));
            xTestSuite.Add(new XElement("system-err"));

            if (_timeStamp.HasValue)
                xTestSuite.Add(new XAttribute("timestamp", _timeStamp.Value.ToJUnitDateTime()));

            _xJUnit.Add(xTestSuite);
        }
        //---------------------------------------------------------------------
        private void AddTest(XElement xTestSuite, TestDefinition test)
        {
            IEnumerable<UnitTestResult> unitTestResults = _lookup[test.Id];

            foreach (var unitTestResult in unitTestResults)
            {
                _testCount++;

                var xTestCase = new XElement("testcase");
                xTestSuite.Add(xTestCase);

                xTestCase.Add(new XAttribute("classname", test.TestClass));
                xTestCase.Add(new XAttribute("name"     , unitTestResult.TestName));

                if (!_timeStamp.HasValue) _timeStamp = unitTestResult.StartTime;

                if (unitTestResult.Duration.HasValue)
                {
                    _time += unitTestResult.Duration.Value;
                    xTestCase.Add(new XAttribute("time", (decimal)unitTestResult.Duration.Value.TotalSeconds));
                }

                if (unitTestResult.Outcome == Outcome.NotExecuted)
                    xTestCase.Add(new XElement("skipped"));
                else if (unitTestResult.Outcome == Outcome.Failed)
                {
                    _failures++;
                    xTestCase.Add(new XElement("failure",
                        unitTestResult.StackTrace,
                        new XAttribute("message", unitTestResult.Message),
                        new XAttribute("type"   , "error")
                        )
                    );
                }
            }
        }
        //---------------------------------------------------------------------
        private void ResetCounters()
        {
            _errors    = 0;
            _testCount = 0;
            _failures  = 0;
            _time      = TimeSpan.Zero;
            _timeStamp = null;
        }
    }
}
