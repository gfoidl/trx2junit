#if NETCOREAPP2_1
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;
        //---------------------------------------------------------------------
        public bool ReturnValue { get; }
    }
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) => this.ParameterValue = parameterValue;
        //---------------------------------------------------------------------
        public bool ParameterValue { get; }
    }
}
#endif
