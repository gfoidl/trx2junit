using System.Xml.Linq;

namespace trx2junit
{
    public interface ITestResultXmlBuilder<TTest> where TTest : Models.Test
    {
        TTest Test      { get; }
        XElement Result { get; }

        void Build();
    }
}
