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
        public void Single_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            int actual   = value.AsSpan().Parse2DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("10")]
        [TestCase("99")]
        public void Two_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            int actual   = value.AsSpan().Parse2DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
    }
}
