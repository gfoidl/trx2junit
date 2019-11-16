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
