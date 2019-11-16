using NUnit.Framework;

namespace trx2junit.Tests.Extensions.IntExtensionsTests
{
    [TestFixture]
    public class Format2DigitIntFast
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(9)]
        public void Lt_10___OK(int value)
        {
            string expected = value.ToString("00");
            string actual   = string.Create(2, value, (buffer, state) => state.Format2DigitIntFast(buffer));

            Assert.AreEqual(expected, actual);
        }
        //---------------------------------------------------------------------
        [Test]
        [TestCase(10)]
        [TestCase(99)]
        public void Ge_10___OK(int value)
        {
            string expected = value.ToString("00");
            string actual   = string.Create(2, value, (buffer, state) => state.Format2DigitIntFast(buffer));

            Assert.AreEqual(expected, actual);
        }
    }
}
