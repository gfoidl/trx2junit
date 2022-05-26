// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Models.JUnit;

internal sealed class JUnitError
{
    public string? Message    { get; set; }
    public string? Type       { get; set; }
    public string? StackTrace { get; set; }
}
