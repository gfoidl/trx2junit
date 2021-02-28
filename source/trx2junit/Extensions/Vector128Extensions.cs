using System.Runtime.CompilerServices;

#if !NETCOREAPP2_1
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace trx2junit
{
#if !NETCOREAPP2_1
    internal static class Vector128Extensions
    {
        private static readonly Vector128<float> s_two     = Vector128.Create(2.00000051757f);
        private static readonly Vector128<short> s_lo_mask = Vector128.Create((ushort)0xFFFF).AsInt16();
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<short> Divide(this Vector128<short> dividend, Vector128<short> divisor)
        {
            // Based on https://stackoverflow.com/a/51458507/347870

            // Convert to two 32-bit integers
            Vector128<int> a_hi_epi32       = Sse2.ShiftRightArithmetic(dividend.AsInt32(), 16);
            Vector128<int> a_lo_epi32_shift = Sse2.ShiftLeftLogical(dividend.AsInt32(), 16);
            Vector128<int> a_lo_epi32       = Sse2.ShiftRightArithmetic(a_lo_epi32_shift, 16);

            Vector128<int> b_hi_epi32       = Sse2.ShiftRightArithmetic(divisor.AsInt32(), 16);
            Vector128<int> b_lo_epi32_shift = Sse2.ShiftLeftLogical(divisor.AsInt32(), 16);
            Vector128<int> b_lo_epi32       = Sse2.ShiftRightArithmetic(b_lo_epi32_shift, 16);

            // Convert to 32-bit floats
            Vector128<float> a_hi = Sse2.ConvertToVector128Single(a_hi_epi32);
            Vector128<float> a_lo = Sse2.ConvertToVector128Single(a_lo_epi32);
            Vector128<float> b_hi = Sse2.ConvertToVector128Single(b_hi_epi32);
            Vector128<float> b_lo = Sse2.ConvertToVector128Single(b_lo_epi32);

            // Calculate the reciprocal
            Vector128<float> b_hi_rcp = Sse.Reciprocal(b_hi);
            Vector128<float> b_lo_rcp = Sse.Reciprocal(b_lo);

            // Calculate the inverse
            Vector128<float> b_hi_inv_1;
            Vector128<float> b_lo_inv_1;
            Vector128<float> two = s_two;
            if (Fma.IsSupported)
            {
                b_hi_inv_1 = Fma.MultiplyAddNegated(b_hi_rcp, b_hi, two);
                b_lo_inv_1 = Fma.MultiplyAddNegated(b_lo_rcp, b_lo, two);
            }
            else
            {
                Vector128<float> b_mul_hi = Sse.Multiply(b_hi_rcp, b_hi);
                Vector128<float> b_mul_lo = Sse.Multiply(b_lo_rcp, b_lo);
                b_hi_inv_1                = Sse.Subtract(two, b_mul_hi);
                b_lo_inv_1                = Sse.Subtract(two, b_mul_lo);
            }

            // Compensate for the loss
            Vector128<float> b_hi_rcp_1 = Sse.Multiply(b_hi_rcp, b_hi_inv_1);
            Vector128<float> b_lo_rcp_1 = Sse.Multiply(b_lo_rcp, b_lo_inv_1);

            // Perform the division by multiplication
            Vector128<float> hi = Sse.Multiply(a_hi, b_hi_rcp_1);
            Vector128<float> lo = Sse.Multiply(a_lo, b_lo_rcp_1);

            // Convert back to integers
            Vector128<int> hi_epi32 = Sse2.ConvertToVector128Int32WithTruncation(hi);
            Vector128<int> lo_epi32 = Sse2.ConvertToVector128Int32WithTruncation(lo);

            // Zero-out the unnecessary parts
            Vector128<int> hi_epi32_shift = Sse2.ShiftLeftLogical(hi_epi32, 16);

            // Blend the bits, and return
            if (Sse41.IsSupported)
            {
                return Sse41.Blend(lo_epi32.AsInt16(), hi_epi32_shift.AsInt16(), 0xAA);
            }
            else
            {
                Vector128<int> lo_epi32_mask = Sse2.And(lo_epi32, s_lo_mask.AsInt32());
                return Sse2.Or(hi_epi32_shift, lo_epi32_mask).AsInt16();
            }
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<short> DivideBy10(this Vector128<short> dividend)
        {
            // Convert to two 32-bit integers
            Vector128<int> a_hi = Sse2.ShiftRightArithmetic(dividend.AsInt32(), 16);
            Vector128<int> a_lo = Sse2.ShiftLeftLogical(dividend.AsInt32(), 16);
            a_lo                = Sse2.ShiftRightArithmetic(a_lo, 16);

            Vector128<int> div10_hi;
            Vector128<int> div10_lo;

            if (Avx2.IsSupported)
            {
                Vector256<int> a      = Vector256.Create(a_lo, a_hi);
                Vector256<int> s0     = Avx2.ShiftRightArithmetic(a, 15);
                Vector256<int> factor = Vector256.Create(26215);
                Vector256<int> mul    = Avx2.MultiplyLow(a, factor);
                Vector256<int> s1     = Avx2.ShiftRightArithmetic(mul, 18);
                Vector256<int> div10  = Avx2.Subtract(s1, s0);

                div10_hi = div10.GetUpper();
                div10_lo = div10.GetLower();
            }
            else
            {
                Vector128<int> s0_hi = Sse2.ShiftRightArithmetic(a_hi, 15);
                Vector128<int> s0_lo = Sse2.ShiftRightArithmetic(a_lo, 15);

                Vector128<int> factor = Vector128.Create(26215);
                Vector128<int> mul_hi = Sse41.MultiplyLow(a_hi, factor);
                Vector128<int> mul_lo = Sse41.MultiplyLow(a_lo, factor);

                Vector128<int> s1_hi = Sse2.ShiftRightArithmetic(mul_hi, 18);
                Vector128<int> s1_lo = Sse2.ShiftRightArithmetic(mul_lo, 18);

                div10_hi = Sse2.Subtract(s1_hi, s0_hi);
                div10_lo = Sse2.Subtract(s1_lo, s0_lo);
            }

            //div10_hi = Sse2.ShiftLeftLogical(div10_hi, 16);
            div10_hi = Sse2.ShiftLeftLogical128BitLane(div10_hi, 2);
            return Sse41.Blend(div10_lo.AsInt16(), div10_hi.AsInt16(), 0xAA);
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<short> Modulo(this Vector128<short> dividend, Vector128<short> divisor, Vector128<short> divisionResult)
        {
            Vector128<short> tmp = Sse2.MultiplyLow(divisor, divisionResult);
            return Sse2.Subtract(dividend, tmp);
        }
    }
#endif
}
