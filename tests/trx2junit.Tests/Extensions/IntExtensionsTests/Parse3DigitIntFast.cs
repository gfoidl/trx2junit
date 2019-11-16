using System;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.IntExtensionsTests
{
    [TestFixture]
    public class Parse3DigitIntFast
    {
        [Test]
        [TestCase("000")]
        [TestCase("001")]
        [TestCase("009")]
        public void Single_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            int actual   = value.AsSpan().Parse3DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("010")]
        [TestCase("099")]
        public void Two_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            int actual   = value.AsSpan().Parse3DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("100")]
        [TestCase("999")]
        public void Three_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            int actual   = value.AsSpan().Parse3DigitIntFast();

            Assert.AreEqual(expected, actual);
        }
    }
}
