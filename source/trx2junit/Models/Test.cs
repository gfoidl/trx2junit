using System.Collections.Generic;

#nullable enable

namespace trx2junit.Models
{
    public class Test
    {
        public Times?                      Times           { get; set; }
        public ResultSummary?              ResultSummary   { get; set; }
        public ICollection<TestDefinition> TestDefinitions { get; set; } = new List<TestDefinition>();
        public ICollection<UnitTestResult> UnitTestResults { get; set; } = new List<UnitTestResult>();
    }
}
