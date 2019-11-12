namespace trx2junit
{
    public class TrxResultSummary
    {
        public TrxOutcome Outcome  { get; set; } = TrxOutcome.Passed;
        public string?    StdOut   { get; set; }
        public int        Total    { get; set; }
        public int        Executed { get; set; }
        public int        Passed   { get; set; }
        public int        Failed   { get; set; }
        public int        Errors   { get; set; }
    }
}
