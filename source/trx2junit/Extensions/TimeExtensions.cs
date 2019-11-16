using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if !NETCOREAPP2_1
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace trx2junit
{
    internal static class TimeExtensions
    {
        private static readonly char[] s_template = "0000-00-00T00:00:00".ToCharArray();
        //---------------------------------------------------------------------
        public static string ToJUnitDateTime(this DateTime dt)
        {
            return string.Create(19, dt, (buffer, value) =>
            {
#if NETCOREAPP2_1
                FormatDateTimeScalar(buffer, value);
#else
                if (Sse41.IsSupported)
                {
                    FormatDateTimeSse41(buffer, value);
                }
                else
                {
                    FormatDateTimeScalar(buffer, value);
                }
#endif
            });
        }
        //---------------------------------------------------------------------
        public static string ToTrxDateTime(this DateTime dt)
        {
            dt = dt.ToUniversalTime();
            return $"{dt.ToJUnitDateTime()}.{dt.Millisecond:000}+00:00";
        }
        //---------------------------------------------------------------------
        public static string ToJUnitTime(this double value) => value.ToString("0.000", CultureInfo.InvariantCulture);
        //---------------------------------------------------------------------
        private static void FormatDateTimeScalar(Span<char> buffer, DateTime value)
        {
            Debug.Assert(s_template.Length == buffer.Length);
            s_template.CopyTo(buffer);

            value.Year  .TryFormat(buffer, out int _);
            value.Month .Format2DigitIntFast(buffer.Slice(5));
            value.Day   .Format2DigitIntFast(buffer.Slice(8));
            value.Hour  .Format2DigitIntFast(buffer.Slice(11));
            value.Minute.Format2DigitIntFast(buffer.Slice(14));
            value.Second.Format2DigitIntFast(buffer.Slice(17));
        }
        //---------------------------------------------------------------------
#if !NETCOREAPP2_1
        private static readonly Vector128<short> s_timeTemplateVec = Vector128.Create(0, 0, (short)':', 0, 0, (short)':', 0, 0);
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FormatDateTimeSse41(Span<char> buffer, DateTime value)
        {
            int year   = value.Year;
            int month  = value.Month;
            int day    = value.Day;
            int hour   = value.Hour;
            int minute = value.Minute;
            int second = value.Second;

            ref char bufferRef = ref MemoryMarshal.GetReference(buffer);

            int year2LowerDigits;
            if (year >= 2100)
            {
                int yearHigh = Math.DivRem(year, 100, out year2LowerDigits);
                yearHigh.Format2DigitIntFast(buffer);
            }
            else
            {
                year2LowerDigits = year - 2000;
                //Unsafe.Add(ref bufferRef, 0) = '2';
                //Unsafe.Add(ref bufferRef, 1) = '0';
                Unsafe.As<char, int>(ref bufferRef) = '0' << 16 | '2';
            }

            Vector128<short> ten      = Vector128.Create((short)10);
            Vector128<short> template = s_timeTemplateVec;
            Vector128<short> ascii0   = Vector128.Create((short)'0');

            Vector128<short> formattedDate = FormatDateTimeComponents(year2LowerDigits, month, day, ten, template, ascii0);
            Unsafe.As<char, Vector128<short>>(ref Unsafe.Add(ref bufferRef, 2)) = formattedDate;
            Unsafe.Add(ref bufferRef, 4) = '-';
            Unsafe.Add(ref bufferRef, 7) = '-';
            Unsafe.Add(ref bufferRef, 10) = 'T';

            Vector128<short> formattedTime = FormatDateTimeComponents(hour, minute, second, ten, template, ascii0);
            Unsafe.As<char, Vector128<short>>(ref Unsafe.Add(ref bufferRef, 11)) = formattedTime;
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector128<short> FormatDateTimeComponents(
            int a,
            int b,
            int c,
            Vector128<short> ten,
            Vector128<short> template,
            Vector128<short> ascii0)
        {
            Vector128<short> vec = Vector128<short>.Zero;
            vec = vec.WithElement(1, (short)a);
            vec = vec.WithElement(4, (short)b);
            vec = vec.WithElement(7, (short)c);

            Vector128<short> div10 = vec.DivideBy10();
            Vector128<short> mod10 = vec.Modulo(ten, div10);

            // Both shifts are equivalent
            Vector128<short> div10Shifted = Sse2.ShiftRightLogical128BitLane(div10, 2);
            //Vector128<short> div10Shifted = Sse2.ShiftRightLogical(div10.AsInt32(), 16).AsInt16();

            Vector128<short> res = Sse2.Or(div10Shifted, mod10);
            res                  = Sse2.Add(res, ascii0);
            res                  = Sse2.Or(res, template);  // '0' | ':' = ':' -- 48 | 58 = 58

            return res;
        }
#endif
    }
}
