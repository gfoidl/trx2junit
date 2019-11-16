using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace trx2junit
{
    internal static class IntExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Format2DigitIntFast(this int value, Span<char> buffer)
        {
            Debug.Assert(0 <= value && value < 100);

            if (value < 10)
            {
                buffer[1] = (char)('0' + value);
                buffer[0] = '0';
            }
            else
            {
                int high = Math.DivRem(value, 10, out int low);

                buffer[1] = (char)('0' + low);
                buffer[0] = (char)('0' + high);
            }
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parse2DigitIntFast(this ReadOnlySpan<char> value)
        {
            Debug.Assert(value.Length >= 2);

            char high = value[0];
            char low  = value[1];

            int h = high - '0';
            int l = low  - '0';

            return h * 10 + l;
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Parse3DigitIntFast(this ReadOnlySpan<char> value)
        {
            Debug.Assert(value.Length >= 3);

            char hundred = value[0];
            char ten     = value[1];
            char single  = value[2];

            int h = hundred - '0';
            int t = ten     - '0';
            int s = single  - '0';

            return h * 100 + t * 10 + s;
        }
    }
}
