using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TimeExtensionsTests
{
    [TestFixture]
    public class ToJUnitDateTime
    {
        [Test, TestCaseSource(nameof(DateTime_given___correct_format_TestCases))]
        public string DateTime_given___correct_format(DateTime dateTime)
        {
            return dateTime.ToJUnitDateTime();
        }
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> DateTime_given___correct_format_TestCases()
        {
            yield return new TestCaseData(new DateTime(2019, 11, 10, 15, 33, 27)).Returns("2019-11-10T15:33:27");
        }
    }
}
