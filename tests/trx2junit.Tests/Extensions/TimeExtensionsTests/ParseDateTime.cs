using System;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TimeExtensionsTests
{
    [TestFixture]
    public class ParseDateTime
    {
        [Test]
        public void Valid_JUnitDateTime___OK()
        {
            DateTime now = new DateTime(2019, 11, 10, 15, 33, 27);
            string value = now.ToJUnitDateTime();

            DateTime? actual = value.ParseDateTime();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.HasValue);
                Assert.AreEqual(now, actual.Value);
            });
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
                        DateTime dt  = new DateTime(year, month, day, hour, minute, second);
                        string value = dt.ToJUnitDateTime();

                        DateTime? actual = value.ParseDateTime();

                        Assert.Multiple(() =>
                        {
                            Assert.IsTrue(actual.HasValue);
                            Assert.AreEqual(dt, actual.Value, "failure at {0}", dt);
                        });
                    }
                }
            }
        }
        //---------------------------------------------------------------------
        [Test]
        public void Valid_TrxDateTime___OK()
        {
            DateTime now = new DateTime(2019, 11, 10, 15, 33, 27, 123);
            string value = now.ToTrxDateTime();

            DateTime? actual = value.ParseDateTime();

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual.HasValue);
                Assert.AreEqual(now.ToUniversalTime(), actual.Value);
                Assert.AreEqual(DateTimeKind.Utc, actual.Value.Kind);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        public void Invalid_DateTime_string___null()
        {
            string value = "123";

            DateTime? actual = value.ParseDateTime();

            Assert.IsFalse(actual.HasValue);
        }
    }
}
