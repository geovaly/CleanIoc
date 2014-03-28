using CleanIoc.Compiler;
using CleanIoc.Expressions;
using CleanIoc.Expressions.Impl;
using CleanIoc.Expressions.Impl.Builders;
using CleanIoc.Factory;
using CleanIoc.Factory.Impl;
using CleanIoc.Factory.Impl.Constants;
using CleanIoc.Factory.Impl.InstanceLookups;
using CleanIoc.Policy;
using CleanIoc.Policy.Impl;
using CleanIoc.Registrations.Impl.GroupByServiceType;
using CleanIoc.Registrations.Impl.GroupByServiceType.Impl;

namespace CleanIoc.CompositeRoot
{
    class DefaultBuilderParamProvider : BuilderParamProvider
    {
        internal override IConstructorSelectorPolicy ConstructorSelectorPolicy()
        {
            return new SingleConstructorSelectorPolicy();
        }

        internal override ITagDetailsBuilder TagDetailsBuilder()
        {
            return new DictionaryTagDetailsBuilder();
        }

        internal override IInstanceExpressionsBuilder InstanceExpressionsBuilder()
        {
            return new LazyInstanceExpressionsBuilder();
        }

        internal override IGroupingRegistrationFactory GroupingRegistrationFactory()
        {
            return new DictionaryGroupingRegistrationFactory();
        }

        internal override ILambdaCompiler LambdaCompiler()
        {
            return new DefaultLambdaCompiler();
        }

        internal override IConstantsBuilder ConstantsBuilder()
        {
            return new DefaultConstantsBuilder();
        }

        internal override IInstanceLookupsBuilder InstanceLookupsBuilder()
        {
            return new DictionaryInstanceLookupsBuilder();
        }
    }
}
