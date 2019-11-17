using System;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.IntExtensionsTests
{
    [TestFixture]
    public class TryParse2DigitIntFast
    {
        [Test]
        [TestCase("00")]
        [TestCase("01")]
        [TestCase("09")]
        [TestCase("10")]
        [TestCase("99")]
        public void Two_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            bool actual  = value.AsSpan().TryParse2DigitIntFast(out int res);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual);
                Assert.AreEqual(expected, res);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("0a")]
        [TestCase("a0")]
        [TestCase("0/")]    // '0' - 1 = '/'
        [TestCase("/0")]
        [TestCase("0:")]    // '9' + 1 = ':'
        [TestCase(":0")]
        public void Non_digit_chars___false(string value)
        {
            bool actual = value.AsSpan().TryParse2DigitIntFast(out int _);

            Assert.IsFalse(actual);
        }
    }
}
