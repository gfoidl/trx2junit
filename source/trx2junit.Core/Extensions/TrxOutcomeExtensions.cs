// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using gfoidl.Trx2Junit.Core.Models.Trx;

namespace gfoidl.Trx2Junit.Core;

internal static class TrxOutcomeExtensions
{
    private static readonly HashSet<TrxOutcome> s_successStates = new()     // :-( capacity can't be set on NS2.0
    {
        TrxOutcome.Completed,
        TrxOutcome.Passed,
        TrxOutcome.PassedButRunAborted
    };

    private static readonly HashSet<TrxOutcome> s_skippedStates = new()
    {
        TrxOutcome.NotExecuted,
        TrxOutcome.NotRunnable,
        TrxOutcome.Disconnected
    };

    private static readonly HashSet<TrxOutcome> s_failureStates = new()
    {
        TrxOutcome.Aborted,
        TrxOutcome.Error,
        TrxOutcome.Failed,
        TrxOutcome.Timeout
    };
    //-------------------------------------------------------------------------
    public static bool IsSuccess(this TrxOutcome? value) => value.HasValue && s_successStates.Contains(value.Value);
    public static bool IsSkipped(this TrxOutcome? value) => !value.HasValue || s_skippedStates.Contains(value.Value);
    public static bool IsFailure(this TrxOutcome? value) => value.HasValue && s_failureStates.Contains(value.Value);
}
