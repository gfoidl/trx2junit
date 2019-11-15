using System;
using System.Globalization;

namespace trx2junit
{
    internal static class TimeExtensions
    {
        public static string ToJUnitDateTime(this DateTime dt) => dt.ToString("s");
        //---------------------------------------------------------------------
        public static string ToTrxDateTime(this DateTime dt)
        {
            dt = dt.ToUniversalTime();
            return $"{dt.ToJUnitDateTime()}.{dt.Millisecond:000}+00:00";
        }
        //---------------------------------------------------------------------
        public static string ToJUnitTime(this double value) => value.ToString("0.000", CultureInfo.InvariantCulture);
    }
}
