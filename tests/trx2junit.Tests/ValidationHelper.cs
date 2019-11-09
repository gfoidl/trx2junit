using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace trx2junit.Tests
{
    internal static class ValidationHelper
    {
        private static readonly XmlSchemaSet s_schemaJunit = LoadSchema("./data/junit.xsd");
        private static readonly XmlSchemaSet s_schemaTrx   = LoadSchema("./data/vstst.xsd");
        //---------------------------------------------------------------------
        public static void IsXmlValidJunit(string fileName, bool validateJunit = true)
        {
            XDocument xml          = XDocument.Load(fileName);
            XmlSchemaSet xmlSchema = validateJunit
                ? s_schemaJunit
                : s_schemaTrx;

            xml.Validate(xmlSchema, (o, e) => throw e.Exception);
        }
        //---------------------------------------------------------------------
        private static XmlSchemaSet LoadSchema(string xsdFile)
        {
            var schema = new XmlSchemaSet();

            using (Stream s = File.OpenRead(xsdFile))
            using (XmlReader xr = XmlReader.Create(s))
                schema.Add("", xr);

            return schema;
        }
    }
}
