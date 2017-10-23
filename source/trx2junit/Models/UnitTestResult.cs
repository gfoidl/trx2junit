using System;

namespace trx2junit
{
    public class UnitTestResult
    {
        public Guid     ExecutionId { get; set; }
        public Guid     TestId      { get; set; }
        public string   TestName    { get; set; }
        public TimeSpan Duration    { get; set; }
        public DateTime StartTime   { get; set; }
        public DateTime EndTime     { get; set; }
        public Outcome  Outcome     { get; set; }
        public string   Message     { get; set; }
        public string   StackTrace  { get; set; }
    }
}