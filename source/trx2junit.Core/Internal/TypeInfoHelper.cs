// (c) gfoidl, all rights reserved

using System.Diagnostics.CodeAnalysis;

namespace gfoidl.Trx2Junit.Core;

internal static class TypeInfoHelper
{
    [return: NotNullIfNotNull("name")]
    public static string? StripTypeInfo(this string? name)
    {
        if (name is null) return null;

        int idx = name.LastIndexOf('.');
        if (idx < 0)
        {
            return name;
        }

        return name[(idx + 1)..];
    }
}
