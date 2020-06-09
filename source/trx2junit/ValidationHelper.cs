using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace trx2junit
{
    internal static class ValidationHelper
    {
        private static readonly XmlSchemaSet s_schemaJunit = LoadSchema("./schemas/junit.xsd");
        private static readonly XmlSchemaSet s_schemaTrx   = LoadSchema("./schemas/vstst.xsd", targetNamespace: "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
        //---------------------------------------------------------------------
        public static void IsXmlValidJunit(string fileName, bool validateJunit, TextWriter textWriter)
        {
            XDocument xml          = XDocument.Load(fileName);
            XmlSchemaSet xmlSchema = validateJunit
                ? s_schemaJunit
                : s_schemaTrx;

            xml.Validate(xmlSchema, (o, e) =>
            {
                textWriter.WriteLine($"Message: {e.Message}");
                textWriter.WriteLine($"LineNo: {e.Exception.LineNumber}, LinePos: {e.Exception.LinePosition}");
                textWriter.WriteLine("Object: {0}", o);
                textWriter.WriteLine();
                textWriter.WriteLine(xml);
                throw new XmlSchemaException("See test output for further details", e.Exception);
            });
        }
        //---------------------------------------------------------------------
        private static XmlSchemaSet LoadSchema(string xsdFile, string targetNamespace = "")
        {
            var schema = new XmlSchemaSet();

            using (Stream s = File.OpenRead(xsdFile))
            using (XmlReader xr = XmlReader.Create(s))
                schema.Add(targetNamespace, xr);

            return schema;
        }
    }
}
