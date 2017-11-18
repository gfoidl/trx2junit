using System;
using System.Xml.Linq;

namespace trx2junit
{
    public static class MyExtensions
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
        public static int ReadInt(this XElement element, string attributeName)
        {
            string value = element.Attribute(attributeName).Value;
            return int.Parse(value);
        }
        //---------------------------------------------------------------------
        public static Guid ReadGuid(this XElement element, string attributeName)
        {
            string value = element.Attribute(attributeName).Value;
            return Guid.Parse(value);
        }
        //---------------------------------------------------------------------
        public static string ToJUnitDateTime(this DateTime dt)
        {
            return $"{dt.Year}-{dt.Month:00}-{dt.Day:00}T{dt.Hour:00}:{dt.Minute:00}:{dt.Second:00}";
        }
    }
}