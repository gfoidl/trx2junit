using System;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.IntExtensionsTests
{
    [TestFixture]
    public class TryParse3DigitIntFast
    {
        [Test]
        [TestCase("000")]
        [TestCase("001")]
        [TestCase("009")]
        [TestCase("010")]
        [TestCase("099")]
        [TestCase("100")]
        [TestCase("999")]
        public void Three_digit_string___OK(string value)
        {
            int expected = int.Parse(value);
            bool actual  = value.AsSpan().TryParse3DigitIntFast(out int res);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(actual);
                Assert.AreEqual(expected, res);
            });
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase("00a")]
        [TestCase("0a0")]
        [TestCase("a00")]
        [TestCase("00/")]    // '0' - 1 = '/'
        [TestCase("0/0")]
        [TestCase("/00")]
        [TestCase("00:")]    // '9' + 1 = ':'
        [TestCase("0:0")]
        [TestCase(":00")]
        public void Non_digit_chars___throws_ArgumentOutOfRange(string value)
        {
            bool actual = value.AsSpan().TryParse3DigitIntFast(out int _);

            Assert.IsFalse(actual);
        }
    }
}
