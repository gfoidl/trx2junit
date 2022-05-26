// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Models.Trx;

internal sealed class TrxResultSummary
{
    public TrxOutcome Outcome  { get; set; } = TrxOutcome.Passed;
    public string?    StdOut   { get; set; }
    public int?       Total    { get; set; } = 0;
    public int?       Executed { get; set; } = 0;
    public int?       Passed   { get; set; } = 0;
    public int?       Failed   { get; set; } = 0;
    public int?       Errors   { get; set; } = 0;
}
