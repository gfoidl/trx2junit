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
            yield return new TestCaseData(new DateTime(2019, 01, 02, 05, 03, 07)).Returns("2019-01-02T05:03:07");
        }
        //---------------------------------------------------------------------
        [Test]
        public void Brute_force_validation()
        {
            int year  = 2019;
            int month = 11;
            int day   = 16;

            for (int hour = 0; hour < 24; ++hour)
            {
                for (int minute = 0; minute < 60; ++minute)
                {
                    for (int second = 0; second < 60; ++second)
                    {
                        DateTime dt     = new DateTime(year, month, day, hour, minute, second);
                        string expected = $"{year:0000}-{month:00}-{day:00}T{hour:00}:{minute:00}:{second:00}";
                        string actual   = dt.ToJUnitDateTime();

                        Assert.AreEqual(expected, actual, "failure at {0}", dt);
                    }
                }
            }
        }
    }
}
