﻿using System;

namespace trx2junit
{
    public class TrxTestDefinition
    {
        public Guid    Id          { get; set; }
        public Guid?   ExecutionId { get; set; }
        public string? TestClass   { get; set; }
        public string? TestMethod  { get; set; }
    }
}
