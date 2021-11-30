// (c) gfoidl, all rights reserved

using System;

namespace gfoidl.Trx2Junit.Core.Models.Trx;

internal class TrxTestDefinition
{
    public Guid    Id          { get; set; }
    public Guid?   ExecutionId { get; set; }
    public string? TestClass   { get; set; }
    public string? TestMethod  { get; set; }
}
