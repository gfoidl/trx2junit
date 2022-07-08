// (c) gfoidl, all rights reserved

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.TimeExtensionsTests;

[TestFixture]
public class ToTrxDateTime
{
    [Test, TestCaseSource(nameof(DateTime_given___correct_format_TestCases))]
    public string DateTime_given___correct_format(DateTimeOffset dateTime)
    {
        return dateTime.ToTrxDateTime();
    }
    //-------------------------------------------------------------------------
    private static IEnumerable<TestCaseData> DateTime_given___correct_format_TestCases()
    {
        yield return new TestCaseData(new DateTimeOffset(2019, 11, 10, 15, 33, 27, 446, TimeSpan.FromHours(0d))).Returns("2019-11-10T15:33:27.446+00:00");
        yield return new TestCaseData(new DateTimeOffset(2019, 11, 10, 15, 33, 27, 446, TimeSpan.FromHours(1d))).Returns("2019-11-10T15:33:27.446+01:00");
        yield return new TestCaseData(new DateTimeOffset(2019, 11, 10, 15, 33, 27, 446, TimeSpan.FromHours(2d))).Returns("2019-11-10T15:33:27.446+02:00");
    }
}
