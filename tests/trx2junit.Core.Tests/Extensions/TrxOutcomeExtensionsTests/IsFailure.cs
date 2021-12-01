// (c) gfoidl, all rights reserved

using System.Collections.Generic;
using gfoidl.Trx2Junit.Core.Models.Trx;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.Extensions.TrxOutcomeExtensionsTests;

[TestFixture]
public class IsFailure
{
    [Test, TestCaseSource(nameof(Value_given___correct_result_TestCases))]
    public bool Value_given___correct_result(int? trxOutcome)
    {
        TrxOutcome? value = trxOutcome.HasValue
            ? (TrxOutcome)trxOutcome.Value
            : null;

        return value.IsFailure();
    }
    //-------------------------------------------------------------------------
    private static IEnumerable<TestCaseData> Value_given___correct_result_TestCases()
    {
        yield return new TestCaseData(null).Returns(false);

        yield return new TestCaseData((int)TrxOutcome.Aborted).Returns(true);
        yield return new TestCaseData((int)TrxOutcome.Error)  .Returns(true);
        yield return new TestCaseData((int)TrxOutcome.Failed) .Returns(true);
        yield return new TestCaseData((int)TrxOutcome.Timeout).Returns(true);

        yield return new TestCaseData((int)TrxOutcome.Completed)          .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.Disconnected)       .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.Inconclusive)       .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.InProgress)         .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.NotExecuted)        .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.NotRunnable)        .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.Passed)             .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.PassedButRunAborted).Returns(false);
        yield return new TestCaseData((int)TrxOutcome.Pending)            .Returns(false);
        yield return new TestCaseData((int)TrxOutcome.Warning)            .Returns(false);
    }
}
