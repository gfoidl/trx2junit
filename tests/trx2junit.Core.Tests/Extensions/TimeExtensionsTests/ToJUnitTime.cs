// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.TimeExtensionsTests;

[TestFixture]
public class ToJUnitTime
{
    [Test, TestCaseSource(nameof(Value_given___correct_format_TestCases))]
    public string Value_given___correct_format(double value)
    {
        return value.ToJUnitTime();
    }
    //-------------------------------------------------------------------------
    private static IEnumerable<TestCaseData> Value_given___correct_format_TestCases()
    {
        yield return new TestCaseData(0)     .Returns("0.000");
        yield return new TestCaseData(0.1)   .Returns("0.100");
        yield return new TestCaseData(0.101) .Returns("0.101");
        yield return new TestCaseData(0.1011).Returns("0.101");
    }
}
