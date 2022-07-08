// (c) gfoidl, all rights reserved

using System;

namespace gfoidl.Trx2Junit.Core.Models.Trx;

internal sealed class TrxTimes
{
    public DateTimeOffset? Creation { get; set; }
    public DateTimeOffset? Queuing  { get; set; }
    public DateTimeOffset? Start    { get; set; }
    public DateTimeOffset? Finish   { get; set; }
    //-------------------------------------------------------------------------
    public TimeSpan? RunTime => this.Finish - this.Creation;
}
