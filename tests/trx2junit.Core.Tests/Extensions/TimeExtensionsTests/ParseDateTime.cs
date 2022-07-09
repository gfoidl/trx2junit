// (c) gfoidl, all rights reserved

using System;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.TimeExtensionsTests;

[TestFixture]
public class ParseDateTime
{
    [Test]
    public void Valid_JUnitDateTime___OK()
    {
        DateTimeOffset now = new(2019, 11, 10, 15, 33, 27, TimeSpan.FromHours(1d));
        string value       = now.UtcDateTime.ToJUnitDateTime();

        DateTimeOffset? actual = value.ParseDateTime();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(now, actual.Value);
        });
    }
    //-------------------------------------------------------------------------
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
                    DateTimeOffset dt = new(year, month, day, hour, minute, second, TimeSpan.FromHours(1d));
                    string value      = dt.UtcDateTime.ToJUnitDateTime();

                    DateTimeOffset? actual = value.ParseDateTime();

                    Assert.Multiple(() =>
                    {
                        Assert.IsTrue(actual.HasValue);
                        Assert.AreEqual(dt, actual.Value, "failure at {0}", dt);
                    });
                }
            }
        }
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Valid_TrxDateTime___OK([Values(-1, 0, 1)] int offset)
    {
        DateTimeOffset now = new(2019, 11, 10, 15, 33, 27, 123, TimeSpan.FromHours(offset));
        string value       = now.ToTrxDateTime();

        DateTimeOffset? actual = value.ParseDateTime();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actual.HasValue);
            Assert.AreEqual(now, actual.Value);
        });
    }
    //-------------------------------------------------------------------------
    [Test]
    [TestCase("123")]
    [TestCase("0000-00-00T00:00:00")]
    [TestCase("0000-00-00T00:00:60")]
    [TestCase("2019-11-17T12:26:0a")]
    [TestCase("201a-11-17T12:26:07")]
    public void Invalid_DateTime_string___null(string value)
    {
        DateTimeOffset? actual = value.ParseDateTime();

        Assert.IsFalse(actual.HasValue, "actual = {0}", actual);
    }
}
