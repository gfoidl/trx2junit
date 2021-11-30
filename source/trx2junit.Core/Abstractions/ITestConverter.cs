// (c) gfoidl, all rights reserved

namespace gfoidl.Trx2Junit.Core.Abstractions;

internal interface ITestConverter<TIn, TOut>
    where TIn  : Models.Test
    where TOut : Models.Test
{
    TIn SourceTest { get; }
    TOut Result    { get; }
    //-------------------------------------------------------------------------
    void Convert();
}
