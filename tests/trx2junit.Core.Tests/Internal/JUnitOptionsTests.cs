// (c) gfoidl, all rights reserved

using gfoidl.Trx2Junit.Core.Internal;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Internal;

[TestFixture]
public class JUnitOptionsTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Create___parsed_from_WorkerOptions(bool jUnitMessagesToSystemOut)
    {
        WorkerOptions workerOptions = new(new[] { "a.trx" })
        {
            JUnitMessagesToSystemErr = jUnitMessagesToSystemOut
        };

        JUnitOptions sut = JUnitOptions.Create(workerOptions);

        Assert.AreEqual(jUnitMessagesToSystemOut, sut.JUnitMessagesToSystemErr);
    }
}
