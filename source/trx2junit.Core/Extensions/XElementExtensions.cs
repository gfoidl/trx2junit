// (c) gfoidl, all rights reserved

using System;
using System.Globalization;
using System.Xml.Linq;

namespace gfoidl.Trx2Junit.Core;

internal static class XElementExtensions
{
    public static DateTimeOffset? ReadDateTime(this XElement element, string attributeName)
    {
        string? value = (string?)element.Attribute(attributeName);
        return value!.ParseDateTime();
    }
    //-------------------------------------------------------------------------
    public static TimeSpan? ReadTimeSpan(this XElement element, string attributeName)
    {
        string? value = (string?)element.Attribute(attributeName);

        if (!TimeSpan.TryParse(value, out TimeSpan ts))
            return null;

        return ts;
    }
    //-------------------------------------------------------------------------
    public static int? ReadInt(this XElement element, string attributeName)
    {
        string? value = (string?)element.Attribute(attributeName);

        if (!int.TryParse(value, out int res))
            return null;

        return res;
    }
    //-------------------------------------------------------------------------
    public static double ReadDouble(this XElement element, string attributeName)
    {
        string? value = (string?)element.Attribute(attributeName);

        if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double res))
            throw new Exception($"The required attribute '{attributeName}' does not exists");

        return res;
    }
    //-------------------------------------------------------------------------
    public static Guid ReadGuid(this XElement element, string attributeName)
    {
        string? value = (string?)element.Attribute(attributeName);

        return Guid.TryParse(value, out Guid guid)
            ? guid
            : Guid.Empty;
    }
    //-------------------------------------------------------------------------
    public static bool Write<T>(this XElement element, string attributeName, T? nullable)
        where T : struct
    {
        if (typeof(T) == typeof(DateTime))
        {
            throw new InvalidOperationException($"Use {nameof(WriteTrxDateTime)} method instead");
        }

        if (!nullable.HasValue) return false;

        element.Add(new XAttribute(attributeName, nullable.Value.ToString()!));

        return true;
    }
    //-------------------------------------------------------------------------
    public static bool WriteTrxDateTime(this XElement element, string attributeName, DateTimeOffset? value)
    {
        if (!value.HasValue) return false;

        element.Add(new XAttribute(attributeName, value.Value.ToTrxDateTime()));

        return true;
    }
}
