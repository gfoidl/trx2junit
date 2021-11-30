using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TrxOutcomeExtensionsTests
{
    [TestFixture]
    public class IsSuccess
    {
        [Test, TestCaseSource(nameof(Value_given___correct_result_TestCases))]
        public bool Value_given___correct_result(TrxOutcome? trxOutcome) => trxOutcome.IsSuccess();
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> Value_given___correct_result_TestCases()
        {
            yield return new TestCaseData(TrxOutcome.Completed)          .Returns(true);
            yield return new TestCaseData(TrxOutcome.Passed)             .Returns(true);
            yield return new TestCaseData(TrxOutcome.PassedButRunAborted).Returns(true);

            yield return new TestCaseData(null)                   .Returns(false);
            yield return new TestCaseData(TrxOutcome.Aborted)     .Returns(false);
            yield return new TestCaseData(TrxOutcome.Disconnected).Returns(false);
            yield return new TestCaseData(TrxOutcome.Error)       .Returns(false);
            yield return new TestCaseData(TrxOutcome.Failed)      .Returns(false);
            yield return new TestCaseData(TrxOutcome.Inconclusive).Returns(false);
            yield return new TestCaseData(TrxOutcome.InProgress)  .Returns(false);
            yield return new TestCaseData(TrxOutcome.NotExecuted) .Returns(false);
            yield return new TestCaseData(TrxOutcome.NotRunnable) .Returns(false);
            yield return new TestCaseData(TrxOutcome.Pending)     .Returns(false);
            yield return new TestCaseData(TrxOutcome.Timeout)     .Returns(false);
            yield return new TestCaseData(TrxOutcome.Warning)     .Returns(false);
        }
    }
}
