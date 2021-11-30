using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TrxOutcomeExtensionsTests
{
    [TestFixture]
    public class IsFailure
    {
        [Test, TestCaseSource(nameof(Value_given___correct_result_TestCases))]
        public bool Value_given___correct_result(TrxOutcome? trxOutcome) => trxOutcome.IsFailure();
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> Value_given___correct_result_TestCases()
        {
            yield return new TestCaseData(TrxOutcome.Aborted).Returns(true);
            yield return new TestCaseData(TrxOutcome.Error)  .Returns(true);
            yield return new TestCaseData(TrxOutcome.Failed) .Returns(true);
            yield return new TestCaseData(TrxOutcome.Timeout).Returns(true);

            yield return new TestCaseData(null)                          .Returns(false);
            yield return new TestCaseData(TrxOutcome.Completed)          .Returns(false);
            yield return new TestCaseData(TrxOutcome.Disconnected)       .Returns(false);
            yield return new TestCaseData(TrxOutcome.Inconclusive)       .Returns(false);
            yield return new TestCaseData(TrxOutcome.InProgress)         .Returns(false);
            yield return new TestCaseData(TrxOutcome.NotExecuted)        .Returns(false);
            yield return new TestCaseData(TrxOutcome.NotRunnable)        .Returns(false);
            yield return new TestCaseData(TrxOutcome.Passed)             .Returns(false);
            yield return new TestCaseData(TrxOutcome.PassedButRunAborted).Returns(false);
            yield return new TestCaseData(TrxOutcome.Pending)            .Returns(false);
            yield return new TestCaseData(TrxOutcome.Warning)            .Returns(false);
        }
    }
}
