using System;

namespace trx2junit
{
    public class Times
    {
        public DateTime? Creation { get; set; }
        public DateTime? Queuing  { get; set; }
        public DateTime? Start    { get; set; }
        public DateTime? Finish   { get; set; }
        //---------------------------------------------------------------------
        public TimeSpan? RunTime => this.Finish - this.Start;
    }
}