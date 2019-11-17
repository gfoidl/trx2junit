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
            Debug.Assert(buffer.Length >= 2);

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
        public static bool TryParse2DigitIntFast(this ReadOnlySpan<char> value, out int res)
        {
            Debug.Assert(value.Length >= 2);

            char low  = value[1];
            char high = value[0];

            if (IsCharDigit(low) && IsCharDigit(high))
            {
                int h = high - '0';
                int l = low  - '0';

                res = h * 10 + l;
                return true;
            }

            res = default;
            return false;
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse3DigitIntFast(this ReadOnlySpan<char> value, out int res)
        {
            Debug.Assert(value.Length >= 3);

            char single  = value[2];
            char ten     = value[1];
            char hundred = value[0];

            if (IsCharDigit(single) && (IsCharDigit(ten) && IsCharDigit(hundred)))
            {
                int h = hundred - '0';
                int t = ten     - '0';
                int s = single  - '0';

                res = h * 100 + t * 10 + s;
                return true;
            }

            res = default;
            return false;
        }
        //---------------------------------------------------------------------
        private static bool IsCharDigit(char c) => (uint)(c - '0') <= '9' - '0';
    }
}
