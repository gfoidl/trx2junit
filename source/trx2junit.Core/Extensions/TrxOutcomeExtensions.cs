using System.Collections.Generic;

namespace trx2junit
{
    public static class TrxOutcomeExtensions
    {
        private static readonly HashSet<TrxOutcome> s_successStates = new HashSet<TrxOutcome>(3)
        {
            TrxOutcome.Completed,
            TrxOutcome.Passed,
            TrxOutcome.PassedButRunAborted
        };

        private static readonly HashSet<TrxOutcome> s_skippedStates = new HashSet<TrxOutcome>(3)
        {
            TrxOutcome.NotExecuted,
            TrxOutcome.NotRunnable,
            TrxOutcome.Disconnected
        };

        private static readonly HashSet<TrxOutcome> s_failureStates = new HashSet<TrxOutcome>(4)
        {
            TrxOutcome.Aborted,
            TrxOutcome.Error,
            TrxOutcome.Failed,
            TrxOutcome.Timeout
        };
        //---------------------------------------------------------------------
        public static bool IsSuccess(this TrxOutcome? value) => value.HasValue && s_successStates.Contains(value.Value);
        public static bool IsSkipped(this TrxOutcome? value) => !value.HasValue || s_skippedStates.Contains(value.Value);
        public static bool IsFailure(this TrxOutcome? value) => value.HasValue && s_failureStates.Contains(value.Value);
    }
}
