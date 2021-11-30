using System;

namespace trx2junit
{
    public class TrxUnitTestResult
    {
        public Guid        ExecutionId  { get; set; }
        public Guid        TestId       { get; set; }
        public string?     TestName     { get; set; }
        public TimeSpan?   Duration     { get; set; }
        public DateTime?   StartTime    { get; set; }
        public DateTime?   EndTime      { get; set; }
        public TrxOutcome? Outcome      { get; set; }
        public string?     ComputerName { get; set; }
        public string?     Message      { get; set; }
        public string?     StackTrace   { get; set; }
        public string?     StdOut       { get; set; }
        public string?     StdErr       { get; set; }
    }
}
