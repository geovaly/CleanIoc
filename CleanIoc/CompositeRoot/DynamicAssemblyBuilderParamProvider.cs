using CleanIoc.Compiler;
using CleanIoc.Expressions;
using CleanIoc.Factory;
using CleanIoc.Factory.Impl.Constants;

namespace CleanIoc.CompositeRoot
{
    class DynamicAssemblyBuilderParamProvider : DefaultBuilderParamProvider
    {
        internal override ILambdaCompiler LambdaCompiler()
        {
            return new DynamicAssemblyLambdaCompiler();
        }

        internal override IConstantsBuilder ConstantsBuilder()
        {
            return new ContainerConstantsBuilder();
        }
    }
}
