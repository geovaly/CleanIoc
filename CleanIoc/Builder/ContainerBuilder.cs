using System;
using System.Linq;
using CleanIoc.CompositeRoot;
using CleanIoc.Registrations;
using CleanIoc.Utility;

namespace CleanIoc.Builder
{
    public class ContainerBuilder : BaseBuilder<IContainer>
    {
        private static bool _highPerformanceBuilderWasCreated;

        private readonly CompositeRegistration _registrations = new CompositeRegistration();
        private readonly IContainerFactory _containerFactory;
        private readonly IRegistrationFactory _registrationFactory;

        public ContainerBuilder()
            : this(new DefaultBuilderParamProvider().Get())
        {
        }

        /// <summary>
        ///  Creates a ContainerBuilder that builds a high-performance container.
        ///  The built container will use some memory that can't be released.  
        ///  You can call this method only one time. Use it only if the built 
        ///  container is used per entire lifetime of the application. 
        /// </summary>
        public static ContainerBuilder CreateHighPerformanceContainerBuilder()
        {
            if (_highPerformanceBuilderWasCreated)
                throw new InvalidOperationException("You can CreateHighPerformanceBuilder only one time");

            _highPerformanceBuilderWasCreated = true;
            return new ContainerBuilder(new HighPerformanceBuilderParamProvider().Get());
        }

        internal ContainerBuilder(BuilderParam param)
            : this(param.ContainerFactory, param.RegistrationFactory)
        {
        }

        internal ContainerBuilder(
            IContainerFactory containerFactory,
            IRegistrationFactory registrationFactory)
        {
            _containerFactory = containerFactory;
            _registrationFactory = registrationFactory;

            _registrations.Add(_registrationFactory.MakeForLifetimeScope());
        }

        public ContainerBuilder RegisterType<TService, TImpl>(Lifestyle lifestyle)
            where TService : class
            where TImpl : class, TService
        {
            EnsureWasNotBuilt();
            AddRegistrationsFrom(lifestyle, _registrationFactory.MakeFactoryFor<TService, TImpl>());
            return this;
        }

        public ContainerBuilder RegisterType<TService>(Func<ILifetimeScope, TService> instanceLookup, Lifestyle lifestyle)
            where TService : class
        {
            EnsureWasNotBuilt();
            AddRegistrationsFrom(lifestyle, _registrationFactory.MakeFactoryFor(instanceLookup));
            return this;
        }

        public ContainerBuilder RegisterInstance<TInstance>(TInstance instance)
             where TInstance : class
        {
            EnsureWasNotBuilt();
            _registrations.Add(_registrationFactory.MakeForInstance(instance));
            return this;
        }

        public ContainerBuilder RegisterStateFromScope<TState>(params object[] tags)
             where TState : class
        {
            if (!tags.Any())
                throw new ArgumentException(@"must not be empty", "tags");

            EnsureWasNotBuilt();
            _registrations.Add(_registrationFactory.MakeForStateScope<TState>(tags));
            return this;
        }

        protected override IContainer OnBuild()
        {
            return _containerFactory.Make(_registrations);
        }

        private void AddRegistrationsFrom(Lifestyle lifestyle, IRegistrationFactoryPerLifestyle factory)
        {
            _registrations.Add(lifestyle.MakeRegistration(factory));
        }
    }
}
