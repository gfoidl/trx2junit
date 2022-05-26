// (c) gfoidl, all rights reserved

using System;
using System.Text;
using System.Xml.Linq;
using gfoidl.Trx2Junit.Core.Abstractions;
using gfoidl.Trx2Junit.Core.Models.JUnit;

namespace gfoidl.Trx2Junit.Core.Internal;

internal sealed class JUnitTestResultXmlBuilder : ITestResultXmlBuilder<JUnitTest>
{
    private readonly JUnitTest _test;
    private readonly XElement  _xJUnit = new("testsuites");
    private StringBuilder?     _junitTestSuiteSystemOutStringBuilder;
    private StringBuilder?     _junitTestSuiteSystemErrStringBuilder;
    //-------------------------------------------------------------------------
    public JUnitTestResultXmlBuilder(JUnitTest test) => _test = test ?? throw new ArgumentNullException(nameof(test));
    //-------------------------------------------------------------------------
    public JUnitTest Test  => _test;
    public XElement Result => _xJUnit;
    //-------------------------------------------------------------------------
    public void Build()
    {
        foreach (JUnitTestSuite testSuite in _test.TestSuites)
        {
            this.AddTestSuite(testSuite);
        }
    }
    //-------------------------------------------------------------------------
    private void AddTestSuite(JUnitTestSuite testSuite)
    {
        var xTestSuite = new XElement("testsuite");

        xTestSuite.Add(new XAttribute("name"    , testSuite.Name!));
        xTestSuite.Add(new XAttribute("hostname", testSuite.HostName ?? "-"));
        xTestSuite.Add(new XAttribute("package" , "not available"));
        xTestSuite.Add(new XAttribute("id"      , testSuite.Id!));

        xTestSuite.Add(new XElement("properties"));

        foreach (JUnitTestCase testCase in testSuite.TestCases)
        {
            this.AddTestCase(xTestSuite, testCase);
        }

        xTestSuite.Add(new XAttribute("tests"    , testSuite.TestCount));
        xTestSuite.Add(new XAttribute("failures" , testSuite.FailureCount!));
        xTestSuite.Add(new XAttribute("errors"   , testSuite.ErrorCount!));
        xTestSuite.Add(new XAttribute("skipped"  , testSuite.SkippedCount!));
        xTestSuite.Add(new XAttribute("time"     , testSuite.TimeInSeconds.ToJUnitTime()));
        xTestSuite.Add(new XAttribute("timestamp", testSuite.TimeStamp.ToJUnitDateTime()));

        if (_junitTestSuiteSystemOutStringBuilder?.Length > 0)
        {
            xTestSuite.Add(new XElement("system-out", _junitTestSuiteSystemOutStringBuilder.ToString().Trim()));
            _junitTestSuiteSystemOutStringBuilder.Clear();
        }
        else
        {
            xTestSuite.Add(new XElement("system-out"));
        }

        if (_junitTestSuiteSystemErrStringBuilder?.Length > 0)
        {
            xTestSuite.Add(new XElement("system-err", _junitTestSuiteSystemErrStringBuilder.ToString().Trim()));
            _junitTestSuiteSystemErrStringBuilder.Clear();
        }
        else
        {
            xTestSuite.Add(new XElement("system-err"));
        }

        _xJUnit.Add(xTestSuite);
    }
    //-------------------------------------------------------------------------
    private void AddTestCase(XElement xTestSuite, JUnitTestCase testCase)
    {
        var xTestCase = new XElement("testcase");
        xTestSuite.Add(xTestCase);

        xTestCase.Add(new XAttribute("name"     , testCase.Name!));
        xTestCase.Add(new XAttribute("classname", testCase.ClassName!));
        xTestCase.Add(new XAttribute("time"     , testCase.TimeInSeconds.ToJUnitTime()));

        if (testCase.Skipped)
        {
            xTestCase.Add(new XElement("skipped"));

            if (Globals.JUnitTestCaseStatusSkipped is not null)
            {
                xTestCase.Add(new XAttribute("status", Globals.JUnitTestCaseStatusSkipped));
            }
        }
        else if (testCase.Error != null)
        {
            xTestCase.Add(new XElement("failure",
                testCase.Error.StackTrace!,
                new XAttribute("message", testCase.Error.Message!),
                new XAttribute("type"   , testCase.Error.Type!)
            ));

            xTestCase.Add(new XAttribute("status", Globals.JUnitTestCaseStatusFailure));
        }
        else
        {
            xTestCase.Add(new XAttribute("status", Globals.JUnitTestCaseStatusSuccess));
        }

        if (testCase.SystemErr != null)
        {
            xTestCase.Add(new XElement("system-err", testCase.SystemErr));

            _junitTestSuiteSystemErrStringBuilder ??= new StringBuilder();
            _junitTestSuiteSystemErrStringBuilder.AppendLine(testCase.SystemErr);
        }

        if (testCase.SystemOut != null)
        {
            xTestCase.Add(new XElement("system-out", testCase.SystemOut));

            _junitTestSuiteSystemOutStringBuilder ??= new StringBuilder();
            _junitTestSuiteSystemOutStringBuilder.AppendLine(testCase.SystemOut);
        }
    }
}
