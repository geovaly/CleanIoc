using System;
using CleanIoc.Builder;
using CleanIoc.Core;
using CleanIoc.Registrations;

namespace CleanIoc.Factory
{
    class ContainerFactory : IContainerFactory
    {
        private readonly Func<IConstantsBuilder> _makeConstantsBuilder;
        private readonly Func<IInstanceLookupsBuilder> _makeInstanceLookupsBuilder;
        private readonly Func<ITagDetailsBuilder> _makeTagDetailsBuilder;

        public ContainerFactory(
            Func<IConstantsBuilder> makeConstantsBuilder,
            Func<IInstanceLookupsBuilder> makeInstanceLookupsBuilder,
            Func<ITagDetailsBuilder> makeTagDetailsBuilder)
        {
            _makeConstantsBuilder = makeConstantsBuilder;
            _makeInstanceLookupsBuilder = makeInstanceLookupsBuilder;
            _makeTagDetailsBuilder = makeTagDetailsBuilder;
        }

        public IContainer Make(IRegistration registration)
        {
            var container = new Container(
                MakeConstants(registration),
                MakeInstanceLookupFinder(registration),
                MakeTagDetailsFinder(registration));

            registration.OnContainerWasBuilt(container);
            return container;
        }

        private ITagDetailsFinder MakeTagDetailsFinder(IRegistration registration)
        {
            var builder = _makeTagDetailsBuilder.Invoke();
            registration.OnInitTags(new TagsInitializer(builder));
            registration.OnInitSingletons(new SingletonsInitializer(builder));
            return builder.Build();
        }

        private IInstanceLookupFinder MakeInstanceLookupFinder(IRegistration registration)
        {
            var builder = _makeInstanceLookupsBuilder.Invoke();
            registration.Accept(builder);
            return builder.Build();
        }

        private object[] MakeConstants(IRegistration registration)
        {
            var builder = _makeConstantsBuilder.Invoke();
            registration.OnMakingConstants(builder);
            return builder.Build();
        }
    }
}
