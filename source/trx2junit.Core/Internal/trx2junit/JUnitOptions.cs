// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Internal;

internal sealed class JUnitOptions
{
    public bool JUnitMessagesToSystemOut { get; set; }
    //-------------------------------------------------------------------------
    public static JUnitOptions Create(WorkerOptions workerOptions)
    {
        return new JUnitOptions
        {
            JUnitMessagesToSystemOut = workerOptions.JUnitMessagesToSystemOut
        };
    }
}
