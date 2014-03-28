using CleanIoc.Builder;
using CleanIoc.Builder.Impl;
using CleanIoc.Expressions;
using CleanIoc.Expressions.Impl;
using CleanIoc.Factory;
using CleanIoc.Policy;
using CleanIoc.Registrations.Impl.GroupByServiceType;
using CleanIoc.Registrations.Impl.TagRegistrations.Reflection;

namespace CleanIoc.CompositeRoot
{
    abstract class BuilderParamProvider
    {
        internal abstract IConstructorSelectorPolicy ConstructorSelectorPolicy();

        internal abstract ILambdaCompiler LambdaCompiler();

        internal abstract IConstantsBuilder ConstantsBuilder();

        internal abstract IInstanceLookupsBuilder InstanceLookupsBuilder();

        internal abstract ITagDetailsBuilder TagDetailsBuilder();

        internal abstract IInstanceExpressionsBuilder InstanceExpressionsBuilder();

        internal abstract IGroupingRegistrationFactory GroupingRegistrationFactory();

        public BuilderParam Get()
        {
            var expressionFinder = new InstanceExpressionFinderWrapper();

            return new BuilderParam(
                RegistrationFactory(expressionFinder),
                GroupByServiceType(PopulateInstanceExpressions(expressionFinder, ContainerFactory())));
        }

        private IContainerFactory ContainerFactory()
        {
            return new ContainerFactory(
                ConstantsBuilder,
                InstanceLookupsBuilder,
                TagDetailsBuilder);
        }

        private IContainerFactory GroupByServiceType(IContainerFactory factory)
        {
            return new GroupByServiceTypeContainerFactory(
                factory,
                new ServiceTypeGrouping(GroupingRegistrationFactory()));
        }

        private IContainerFactory PopulateInstanceExpressions(InstanceExpressionFinderWrapper expressionFinder, IContainerFactory factory)
        {
            return new PopulateInstanceExpressions(
                expressionFinder,
                new InstanceExpressionFinderFactory(InstanceExpressionsBuilder),
                factory);
        }

        private RegistrationFactory RegistrationFactory(InstanceExpressionFinderWrapper expressionFinder)
        {
            var constructorSelector = ConstructorSelectorPolicy();

            return new RegistrationFactory(
                new InstanceFactory(constructorSelector),
                new InstanceExpressionFactory(expressionFinder, constructorSelector),
                new InstanceExpressionCompiler(LambdaCompiler()));
        }
    }

}
