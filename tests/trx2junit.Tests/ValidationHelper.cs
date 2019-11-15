using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using NUnit.Framework;

namespace trx2junit.Tests
{
    internal static class ValidationHelper
    {
        private static readonly XmlSchemaSet s_schemaJunit = LoadSchema("./data/junit.xsd");
        private static readonly XmlSchemaSet s_schemaTrx   = LoadSchema("./data/vstst.xsd", targetNamespace: "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");
        //---------------------------------------------------------------------
        public static void IsXmlValidJunit(string fileName, bool validateJunit)
        {
            XDocument xml          = XDocument.Load(fileName);
            XmlSchemaSet xmlSchema = validateJunit
                ? s_schemaJunit
                : s_schemaTrx;

            xml.Validate(xmlSchema, (o, e) =>
            {
                TestContext.WriteLine($"Message: {e.Message}");
                TestContext.WriteLine($"LineNo: {e.Exception.LineNumber}, LinePos: {e.Exception.LinePosition}");
                TestContext.WriteLine("Object: {0}", o);
                TestContext.WriteLine();
                TestContext.WriteLine(xml);
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
