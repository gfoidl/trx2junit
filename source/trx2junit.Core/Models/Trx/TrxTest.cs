// (c) gfoidl, all rights reserved

using System.Collections.Generic;

namespace gfoidl.Trx2Junit.Core.Models.Trx;

internal sealed class TrxTest : Test
{
    public TrxTimes?                      Times           { get; set; }
    public TrxResultSummary?              ResultSummary   { get; set; }
    public ICollection<TrxTestDefinition> TestDefinitions { get; set; } = new List<TrxTestDefinition>();
    public ICollection<TrxUnitTestResult> UnitTestResults { get; set; } = new List<TrxUnitTestResult>();
}
