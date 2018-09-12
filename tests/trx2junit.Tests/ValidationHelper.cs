using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace trx2junit.Tests
{
    internal static class ValidationHelper
    {
        private static readonly XmlSchemaSet s_schema;
        //---------------------------------------------------------------------
        static ValidationHelper()
        {
            s_schema = new XmlSchemaSet();

            using (Stream s = File.OpenRead("./data/junit.xsd"))
            using (XmlReader xr = XmlReader.Create(s))
                s_schema.Add("", xr);
        }
        //---------------------------------------------------------------------
        public static void IsXmlValidJunit(string fileName)
        {
            XDocument junit = XDocument.Load(fileName);

            junit.Validate(s_schema, (o, e) => throw e.Exception);
        }
    }
}
