using System;
using CleanIoc.Core;
using CleanIoc.Expressions;
using CleanIoc.Registrations;

namespace CleanIoc.Tests.UnitTests.TestDoubles
{
    class FakeRegistration<TService> : IRegistration<TService> where TService : class
    {
        public Action<ITagsInitializer> OnInitTagsCallback = initializer => { };

        public Action<ISingletonsInitializer> OnInitSingletonsCallback = initializer => { };

        public Action<IConstantFactory> OnMakingConstantsCallback = factory => { };

        public Action<Container> OnContainerWasBuiltCallback = container => { };

        public InstanceLookup<TService> MakeInstanceLookupResult { get; set; }

        public InstanceExpression MakeInstanceExpressionResult { get; set; }

        public InstanceLookup<TService> MakeInstanceLookup()
        {
            return MakeInstanceLookupResult;
        }

        public InstanceExpression MakeInstanceExpression()
        {
            return MakeInstanceExpressionResult;
        }

        public void OnInitTags(ITagsInitializer initializer)
        {
            OnInitTagsCallback.Invoke(initializer);
        }

        public void OnInitSingletons(ISingletonsInitializer initializer)
        {
            OnInitSingletonsCallback.Invoke(initializer);
        }

        public void OnMakingConstants(IConstantFactory factory)
        {
            OnMakingConstantsCallback(factory);
        }

        public void OnContainerWasBuilt(Container container)
        {
            OnContainerWasBuiltCallback.Invoke(container);
        }

        public void Accept(IRegistrationVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
