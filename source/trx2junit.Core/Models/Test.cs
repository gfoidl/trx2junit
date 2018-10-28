using System;
using System.Collections.Generic;

namespace trx2junit.Models
{
    public class Test
    {
        public Times                            Times           { get; set; }
        public ResultSummary                    ResultSummary   { get; set; }
        public Dictionary<Guid, TestDefinition> TestDefinitions { get; set; } = new Dictionary<Guid, TestDefinition>();
        public Dictionary<Guid, UnitTestResult> UnitTestResults { get; set; } = new Dictionary<Guid, UnitTestResult>();
    }
}
