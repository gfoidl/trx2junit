// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Models.Trx;

internal enum TrxOutcome
{
    Aborted,
    Completed,
    Disconnected,
    Error,
    Failed,
    Inconclusive,
    InProgress,
    NotExecuted,
    NotRunnable,
    Passed,
    PassedButRunAborted,
    Pending,
    Timeout,
    Warning
}
