#if NETCOREAPP2_1
using System.Runtime.CompilerServices;

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;
        //---------------------------------------------------------------------
        public bool ReturnValue { get; }
    }
}
//-----------------------------------------------------------------------------
namespace System
{
    public readonly struct Index
    {
        private readonly int _value;
        //---------------------------------------------------------------------
        public Index(int value, bool fromEnd = false)
        {
            _value = fromEnd ? ~value : value;
        }
        //---------------------------------------------------------------------
        public bool IsFromEnd => _value < 0;
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Index FromStart(int value)
        {
            return new Index(value);
        }
        //---------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOffset(int length)
        {
            int offset = _value;
            if (this.IsFromEnd)
            {
                // offset = length - (~value)
                // offset = length + (~(~value) + 1)
                // offset = length + value + 1

                offset += length + 1;
            }
            return offset;
        }
        //---------------------------------------------------------------------
        public static implicit operator Index(int value) => FromStart(value);
    }
    //-------------------------------------------------------------------------
    public readonly struct Range
    {
        public Index Start { get; }
        public Index End { get; }
        //---------------------------------------------------------------------
        public Range(Index start, Index end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}
#endif
