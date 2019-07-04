using System.Xml.Linq;

#nullable enable

namespace trx2junit
{
    public interface IJUnitBuilder
    {
        XElement Result { get; }

        void Build();
    }
}
