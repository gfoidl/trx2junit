using System;
using System.Xml.Linq;
using trx2junit.Models;

namespace trx2junit
{
    public class JUnitTestResultXmlBuilder : ITestResultXmlBuilder<JUnitTest>
    {
        private readonly JUnitTest _test;
        private readonly XElement  _xJUnit = new XElement("testsuites");
        //---------------------------------------------------------------------
        public JUnitTestResultXmlBuilder(JUnitTest? test) => _test = test ?? throw new ArgumentNullException(nameof(test));
        //---------------------------------------------------------------------
        public JUnitTest Test  => _test;
        public XElement Result => _xJUnit;
        //---------------------------------------------------------------------
        public void Build()
        {
            foreach (JUnitTestSuite testSuite in _test.TestSuites)
            {
                this.AddTestSuite(testSuite);
            }
        }
        //---------------------------------------------------------------------
        private void AddTestSuite(JUnitTestSuite testSuite)
        {
            var xTestSuite = new XElement("testsuite");

            xTestSuite.Add(new XAttribute("name"    , testSuite.Name));
            xTestSuite.Add(new XAttribute("hostname", testSuite.HostName));
            xTestSuite.Add(new XAttribute("package" , "not available"));
            xTestSuite.Add(new XAttribute("id"      , testSuite.Id));

            xTestSuite.Add(new XElement("properties"));

            foreach (JUnitTestCase testCase in testSuite.TestCases)
            {
                this.AddTestCase(xTestSuite, testCase);
            }

            xTestSuite.Add(new XAttribute("tests"    , testSuite.TestCount));
            xTestSuite.Add(new XAttribute("failures" , testSuite.FailureCount));
            xTestSuite.Add(new XAttribute("errors"   , testSuite.ErrorCount));
            xTestSuite.Add(new XAttribute("skipped"  , testSuite.SkippedCount));
            xTestSuite.Add(new XAttribute("time"     , testSuite.TimeInSeconds.ToJUnitTime()));
            xTestSuite.Add(new XAttribute("timestamp", testSuite.TimeStamp.ToJUnitDateTime()));
            xTestSuite.Add(new XElement("system-out"));
            xTestSuite.Add(new XElement("system-err"));

            _xJUnit.Add(xTestSuite);
        }
        //---------------------------------------------------------------------
        private void AddTestCase(XElement xTestSuite, JUnitTestCase testCase)
        {
            var xTestCase = new XElement("testcase");
            xTestSuite.Add(xTestCase);

            xTestCase.Add(new XAttribute("name"     , testCase.Name));
            xTestCase.Add(new XAttribute("classname", testCase.ClassName));
            xTestCase.Add(new XAttribute("time"     , testCase.TimeInSeconds.ToJUnitTime()));

            if (testCase.Skipped)
            {
                xTestCase.Add(new XElement("skipped"));
            }
            else if (testCase.Error != null)
            {
                xTestCase.Add(new XElement("failure",
                    testCase.Error.StackTrace,
                    new XAttribute("message", testCase.Error.Message),
                    new XAttribute("type"   , testCase.Error.Type)
                ));
            }
        }
    }
}
