// (c) gfoidl, all rights reserved

using System;
using System.Diagnostics.CodeAnalysis;

namespace gfoidl.Trx2Junit.Core;

internal static class TypeInfoHelper
{
    [return: NotNullIfNotNull(nameof(name))]
    public static string? StripTypeInfo(this string? name)
    {
        if (name is null) return null;

        ReadOnlySpan<char> span = name;

        int parenthesisIndex = span.IndexOf('(');
        if (parenthesisIndex < 0)
        {
            return StripTypeInfo(span).ToString();
        }

        ReadOnlySpan<char> preParenthesis   = span.Slice(0, parenthesisIndex);
        ReadOnlySpan<char> parenthisContent = span.Slice(parenthesisIndex);

        preParenthesis = StripTypeInfo(preParenthesis);

#if NET6_0_OR_GREATER
        return string.Concat(preParenthesis, parenthisContent);
#else
        int finalLength = preParenthesis.Length + parenthisContent.Length;
        unsafe
        {
            fixed (char* ptr0 = preParenthesis)
            fixed (char* ptr1 = parenthisContent)
            {
                return string.Create(
                    finalLength,
                    ((IntPtr)ptr0, preParenthesis.Length, (IntPtr)ptr1, parenthisContent.Length),
                    static (buffer, state) =>
                    {
                        ReadOnlySpan<char> s0 = new(state.Item1.ToPointer(), state.Item2);
                        ReadOnlySpan<char> s1 = new(state.Item3.ToPointer(), state.Item4);

                        s0.CopyTo(buffer);
                        s1.CopyTo(buffer.Slice(s0.Length));
                    }
                );
            }
        }
#endif
    }
    //-------------------------------------------------------------------------
    private static ReadOnlySpan<char> StripTypeInfo(ReadOnlySpan<char> name)
    {
        int idx = name.LastIndexOf('.');
        if (idx < 0)
        {
            return name;
        }

        return name.Slice(idx + 1);
    }
}
