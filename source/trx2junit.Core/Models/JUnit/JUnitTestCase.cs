﻿namespace trx2junit.Models
{
    public class JUnitTestCase
    {
        public string?     ClassName     { get; set; }
        public string?     Name          { get; set; }
        public double      TimeInSeconds { get; set; }
        public bool        Skipped       { get; set; }
        public JUnitError? Error         { get; set; }
        public string?     SystemOut     { get; set; }
        public string?     SystemErr     { get; set; }
    }
}
