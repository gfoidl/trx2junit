// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;

namespace gfoidl.Trx2Junit.Core.Models.JUnit;

internal sealed class JUnitTestSuite
{
    public int?     Id                           { get; set; }
    public string?  Name                         { get; set; }
    public DateTime TimeStamp                    { get; set; }
    public string?  HostName                     { get; set; }
    public int      TestCount                    { get; set; }
    public int?     FailureCount                 { get; set; }
    public int?     ErrorCount                   { get; set; }
    public int?     SkippedCount                 { get; set; }
    public double   TimeInSeconds                { get; set; }
    public string?  SystemOut                    { get; set; }
    public string?  SystemErr                    { get; set; }
    public ICollection<JUnitProperty> Properties { get; set; } = new List<JUnitProperty>();
    public ICollection<JUnitTestCase> TestCases  { get; set; } = new List<JUnitTestCase>();
}
