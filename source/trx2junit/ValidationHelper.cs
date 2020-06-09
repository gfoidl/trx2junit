using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
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
        public static bool TryValidateXml(string fileName, bool validateJunit, [NotNullWhen(false)] out string? msg)
        {
            XDocument xml          = XDocument.Load(fileName);
            XmlSchemaSet xmlSchema = validateJunit
                ? s_schemaJunit
                : s_schemaTrx;

            bool result = true;
            var sb      = new StringBuilder();

            xml.Validate(xmlSchema, (o, e) =>
            {
                sb.Append("Message: ").AppendLine(e.Message);
                sb.Append("LineNo: ").Append(e.Exception.LineNumber).Append(", LinePos: ").Append(e.Exception.LinePosition).AppendLine();
                sb.AppendFormat("Object: {0}", o).AppendLine();

                result = false;
            });

            if (result)
            {
                msg = null;
                return true;
            }

            msg = sb.ToString();
            return false;
        }
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
                throw new XmlSchemaException("See output for further details", e.Exception);
            });
        }
        //---------------------------------------------------------------------
        private static XmlSchemaSet LoadSchema(string xsdFile, string targetNamespace = "")
        {
            var schema = new XmlSchemaSet();

            using (Stream s     = File.OpenRead(xsdFile))
            using (XmlReader xr = XmlReader.Create(s))
                schema.Add(targetNamespace, xr);

            return schema;
        }
    }
}
