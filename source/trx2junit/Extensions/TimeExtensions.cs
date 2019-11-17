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
        private static readonly char[] s_junitTimeTemplate   = "0000-00-00T00:00:00"          .ToCharArray();
        private static readonly char[] s_trxDateTimeTemplate = "0000-00-00T00:00:00.000+00:00".ToCharArray();
        //---------------------------------------------------------------------
        public static string ToJUnitDateTime(this DateTime dt)
        {
            return string.Create(19, dt, (buffer, value) => FormatDateTime(buffer, value));
        }
        //---------------------------------------------------------------------
        public static string ToTrxDateTime(this DateTime dt)
        {
            dt = dt.ToUniversalTime();

            return string.Create(19 + 1 + 3 + 6, dt, (buffer, value) =>
            {
                s_trxDateTimeTemplate.CopyTo(buffer);

                FormatDateTime(buffer, value);
                dt.Millisecond.TryFormat(buffer.Slice(20), out int written, "000");
                Debug.Assert(written == 3);
            });
        }
        //---------------------------------------------------------------------
        public static string ToJUnitTime(this double value) => value.ToString("0.000", CultureInfo.InvariantCulture);
        //---------------------------------------------------------------------
        public static DateTime? ParseDateTime(this string value)
        {
            ReadOnlySpan<char> span = value;

            if (span.Length != 19 && span.Length != 29)
            {
                return SlowPath(value);
            }

            try
            {
                int year;
                int month;
                int day;
                int hour;
                int minute;
                int second;
#if NETCOREAPP2_1
                ParseDateTimeScalar(span, out year, out month, out day, out hour, out minute, out second);
#else
                if (Sse41.IsSupported)
                {
                    ParseDateTimeSse41(span, out year, out month, out day, out hour, out minute, out second);
                }
                else
                {
                    ParseDateTimeScalar(span, out year, out month, out day, out hour, out minute, out second);
                }
#endif
                int millisecond           = 0;
                DateTimeKind dateTimeKind = DateTimeKind.Local;

                if (value.Length == 29)
                {
                    millisecond  = span[20..24].Parse3DigitIntFast();
                    dateTimeKind = DateTimeKind.Utc;
                }

                return new DateTime(year, month, day, hour, minute, second, millisecond, dateTimeKind);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
            catch
            {
                return SlowPath(value);
            }
            //-----------------------------------------------------------------
            static DateTime? SlowPath(string value)
            {
                if (!DateTime.TryParse(value, out DateTime dt))
                    return null;

                return dt;
            }
        }
        //---------------------------------------------------------------------
        private static void FormatDateTime(Span<char> buffer, DateTime value)
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
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FormatDateTimeScalar(Span<char> buffer, DateTime value)
        {
            Debug.Assert(s_junitTimeTemplate.Length <= buffer.Length);
            s_junitTimeTemplate.CopyTo(buffer);

            value.Year  .TryFormat(buffer, out int _);
            value.Month .Format2DigitIntFast(buffer.Slice(5));
            value.Day   .Format2DigitIntFast(buffer.Slice(8));
            value.Hour  .Format2DigitIntFast(buffer.Slice(11));
            value.Minute.Format2DigitIntFast(buffer.Slice(14));
            value.Second.Format2DigitIntFast(buffer.Slice(17));
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseDateTimeScalar(
            ReadOnlySpan<char> value,
            out int year,
            out int month,
            out int day,
            out int hour,
            out int minute,
            out int second)
        {
            Debug.Assert(value.Length >= 19);

            year   = value[ 0.. 2].Parse2DigitIntFast() * 100 + value[2..4].Parse2DigitIntFast();
            month  = value[ 5.. 7].Parse2DigitIntFast();
            day    = value[ 8..10].Parse2DigitIntFast();
            hour   = value[11..13].Parse2DigitIntFast();
            minute = value[14..16].Parse2DigitIntFast();
            second = value[17..19].Parse2DigitIntFast();
        }
        //---------------------------------------------------------------------
#if !NETCOREAPP2_1
        private static readonly Vector128<short> s_timeTemplateVec       = Vector128.Create(0, 0, (short)':', 0, 0, (short)':', 0, 0);
        private static readonly Vector128<short> s_timeOutsideMaskVec    = Vector128.Create(0xFF_FF, 0xFF_FF, 0, 0xFF_FF, 0xFF_FF, 0, 0xFF_FF, 0xFF_FF).AsInt16();
        private static readonly Vector128<byte>  s_parsingShuffleMaskVec = Vector128.Create((byte)0, 1, 2, 3, 6, 7, 8, 9, 12, 13, 14, 15, 4, 5, 10, 11);
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void FormatDateTimeSse41(Span<char> buffer, DateTime value)
        {
            Debug.Assert(buffer.Length >= 19);

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
            Unsafe.Add(ref bufferRef, 4)  = '-';
            Unsafe.Add(ref bufferRef, 7)  = '-';
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
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseDateTimeSse41(
            ReadOnlySpan<char> value,
            out int year,
            out int month,
            out int day,
            out int hour,
            out int minute,
            out int second)
        {
            Debug.Assert(value.Length >= 19);

            Vector128<short> ascii0      = Vector128.Create((short)'0');
            Vector128<short> ascii9      = Vector128.Create((short)'9');
            Vector128<short> outsideMask = s_timeOutsideMaskVec;
            Vector128<byte> shuffleMask  = s_parsingShuffleMaskVec;
            Vector128<short> ten         = Vector128.Create((short)10);

            ParseDateTimeComponents(value.Slice(2) , ascii0, ascii9, outsideMask, shuffleMask, ten, out year, out month , out day);
            ParseDateTimeComponents(value.Slice(11), ascii0, ascii9, outsideMask, shuffleMask, ten, out hour, out minute, out second);

            year += value[0..2].Parse2DigitIntFast() * 100;
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseDateTimeComponents(
            ReadOnlySpan<char> value,
            Vector128<short>   ascii0,
            Vector128<short>   ascii9,
            Vector128<short>   outsideMask,
            Vector128<byte>    shuffleMask,
            Vector128<short>   ten,
            out int a,
            out int b,
            out int c)
        {
            Debug.Assert(value.Length >= Vector128<short>.Count);

            ref char valueRef    = ref MemoryMarshal.GetReference(value);
            Vector128<short> vec = Unsafe.As<char, Vector128<short>>(ref valueRef);

            Vector128<short> belowAscii0 = Sse2.CompareLessThan(vec, ascii0);
            Vector128<short> aboveAscii9 = Sse2.CompareGreaterThan(vec, ascii9);
            Vector128<short> outside     = Sse2.Or(belowAscii0, aboveAscii9);
            outside                      = Sse2.And(outside, outsideMask);

            if (Sse2.MoveMask(outside.AsByte()) != 0)
                ThrowArgumentOutOfRange();

            Vector128<short> res = Sse2.Subtract(vec, ascii0);

            res                 = Ssse3.Shuffle(res.AsByte(), shuffleMask).AsInt16();
            Vector128<short> lo = Sse2.ShiftRightLogical128BitLane(res, 2);
            Vector128<short> hi = Sse2.MultiplyLow(res, ten);
            res                 = Sse2.Add(lo, hi);

            a = res.GetElement(0);
            b = res.GetElement(2);
            c = res.GetElement(4);
            //-----------------------------------------------------------------
            static void ThrowArgumentOutOfRange() => throw new ArgumentOutOfRangeException();
        }
#endif
    }
}
