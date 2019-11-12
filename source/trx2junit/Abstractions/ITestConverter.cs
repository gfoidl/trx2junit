namespace trx2junit
{
    public interface ITestConverter<TIn, TOut>
        where TIn  : Models.Test
        where TOut : Models.Test
    {
        TIn SourceTest { get; }
        TOut Result    { get; }
        //---------------------------------------------------------------------
        void Convert();
    }
}
