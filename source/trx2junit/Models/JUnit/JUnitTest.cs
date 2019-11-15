using System.Collections.Generic;

namespace trx2junit.Models
{
    public class JUnitTest : Test
    {
        public IList<JUnitTestSuite> TestSuites { get; set; } = new List<JUnitTestSuite>();
    }
}
