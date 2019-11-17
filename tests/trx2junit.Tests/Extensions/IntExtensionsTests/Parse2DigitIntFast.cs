using System;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.IntExtensionsTests
{
    [TestFixture]
    public class Parse2DigitIntFast
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
            int actual   = value.AsSpan().Parse2DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("0a")]
        [TestCase("a0")]
        [TestCase("0/")]    // '0' - 1 = '/'
        [TestCase("/0")]
        [TestCase("0:")]    // '9' + 1 = ':'
        [TestCase(":0")]
        public void Non_digit_chars___throws_ArgumentOutOfRange(string value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => value.AsSpan().Parse2DigitIntFast());
        }
    }
}
