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
    }
}
