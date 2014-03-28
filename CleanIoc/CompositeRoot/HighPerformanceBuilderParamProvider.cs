using CleanIoc.Factory;
using CleanIoc.Factory.Impl.InstanceLookups;

namespace CleanIoc.CompositeRoot
{
    class HighPerformanceBuilderParamProvider : DynamicAssemblyBuilderParamProvider
    {
        internal override IInstanceLookupsBuilder InstanceLookupsBuilder()
        {
            return HighPerformanceInstanceLookupsBuilder.Instance;
        }
    }
}
