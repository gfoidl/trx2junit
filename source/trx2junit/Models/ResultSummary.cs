namespace trx2junit
{
    public class ResultSummary
    {
        public Outcome Outcome  { get; set; }
        public string  StdOut   { get; set; }
        public int     Total    { get; set; }
        public int     Executed { get; set; }
        public int     Passed   { get; set; }
        public int     Failed   { get; set; }
        public int     Error    { get; set; }
    }
}
