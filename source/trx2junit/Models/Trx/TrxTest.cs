using System.Collections.Generic;

namespace trx2junit.Models
{
    public class TrxTest : Test
    {
        public TrxTimes?                      Times           { get; set; }
        public TrxResultSummary?              ResultSummary   { get; set; }
        public ICollection<TrxTestDefinition> TestDefinitions { get; set; } = new List<TrxTestDefinition>();
        public ICollection<TrxUnitTestResult> UnitTestResults { get; set; } = new List<TrxUnitTestResult>();
    }
}
