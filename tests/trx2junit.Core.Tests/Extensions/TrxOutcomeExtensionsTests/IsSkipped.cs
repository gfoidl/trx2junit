using System.Collections.Generic;
using NUnit.Framework;

namespace trx2junit.Tests.Extensions.TrxOutcomeExtensionsTests
{
    [TestFixture]
    public class IsSkipped
    {
        [Test, TestCaseSource(nameof(Value_given___correct_result_TestCases))]
        public bool Value_given___correct_result(TrxOutcome? trxOutcome) => trxOutcome.IsSkipped();
        //---------------------------------------------------------------------
        private static IEnumerable<TestCaseData> Value_given___correct_result_TestCases()
        {
            yield return new TestCaseData(null)                   .Returns(true);
            yield return new TestCaseData(TrxOutcome.NotExecuted) .Returns(true);
            yield return new TestCaseData(TrxOutcome.NotRunnable) .Returns(true);
            yield return new TestCaseData(TrxOutcome.Disconnected).Returns(true);

            yield return new TestCaseData(TrxOutcome.Aborted)            .Returns(false);
            yield return new TestCaseData(TrxOutcome.Completed)          .Returns(false);
            yield return new TestCaseData(TrxOutcome.Error)              .Returns(false);
            yield return new TestCaseData(TrxOutcome.Failed)             .Returns(false);
            yield return new TestCaseData(TrxOutcome.Inconclusive)       .Returns(false);
            yield return new TestCaseData(TrxOutcome.InProgress)         .Returns(false);
            yield return new TestCaseData(TrxOutcome.Passed)             .Returns(false);
            yield return new TestCaseData(TrxOutcome.PassedButRunAborted).Returns(false);
            yield return new TestCaseData(TrxOutcome.Pending)            .Returns(false);
            yield return new TestCaseData(TrxOutcome.Timeout)            .Returns(false);
            yield return new TestCaseData(TrxOutcome.Warning)            .Returns(false);
        }
    }
}
