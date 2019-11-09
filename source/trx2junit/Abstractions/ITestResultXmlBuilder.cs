using System.Xml.Linq;

namespace trx2junit
{
    public interface ITestResultXmlBuilder
    {
        XElement Result { get; }

        void Build();
    }
}
