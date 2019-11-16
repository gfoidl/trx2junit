using System;
using System.Xml.Linq;

namespace trx2junit
{
    internal static class XElementExtensions
    {
        public static DateTime? ReadDateTime(this XElement element, string attributeName)
        {
            string value = (string)element.Attribute(attributeName);

            if (!DateTime.TryParse(value, out DateTime dt))
                return null;

            return dt;
        }
        //---------------------------------------------------------------------
        public static TimeSpan? ReadTimeSpan(this XElement element, string attributeName)
        {
            string value = (string)element.Attribute(attributeName);

            if (!TimeSpan.TryParse(value, out TimeSpan ts))
                return null;

            return ts;
        }
        //---------------------------------------------------------------------
        public static int? ReadInt(this XElement element, string attributeName)
        {
            string value = (string)element.Attribute(attributeName);

            if (!int.TryParse(value, out int res))
                return null;

            return res;
        }
        //---------------------------------------------------------------------
        public static double ReadDouble(this XElement element, string attributeName)
        {
            return (double)element.Attribute(attributeName);
        }
        //---------------------------------------------------------------------
        public static Guid ReadGuid(this XElement element, string attributeName)
        {
            return (Guid)element.Attribute(attributeName);
        }
        //---------------------------------------------------------------------
        public static bool Write<T>(this XElement element, string attributeName, T? nullable)
            where T : struct
        {
            if (typeof(T) == typeof(DateTime))
            {
                throw new InvalidOperationException($"Use {nameof(WriteTrxDateTime)} method instead");
            }

            if (!nullable.HasValue) return false;

            element.Add(new XAttribute(attributeName, nullable.Value.ToString()));

            return true;
        }
        //---------------------------------------------------------------------
        public static bool WriteTrxDateTime(this XElement element, string attributeName, DateTime? value)
        {
            if (!value.HasValue) return false;

            element.Add(new XAttribute(attributeName, value.Value.ToTrxDateTime()));

            return true;
        }
    }
}
