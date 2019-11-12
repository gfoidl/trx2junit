using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TimeExtensionsTests
{
    [TestFixture]
    public class ToTrxDateTime
    {
        [Test, TestCaseSource(nameof(DateTime_given___correct_format_TestCases))]
        public string DateTime_given___correct_format(DateTime dateTime)
        {
            return dateTime.ToTrxDateTime();
        }
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> DateTime_given___correct_format_TestCases()
        {
            yield return new TestCaseData(new DateTime(2019, 11, 10, 15, 33, 27, 446, DateTimeKind.Utc)).Returns("2019-11-10T15:33:27.446+00:00");
        }
    }
}
