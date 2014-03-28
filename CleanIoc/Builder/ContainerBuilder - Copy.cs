//using System;
//using Ioc19.Registrations;
//using Ioc19.Utility;

//namespace Ioc19.Builder
//{
//    public class ContainerBuilder : BaseBuilder<IContainer>
//    {
//        private readonly CompositeRegistration _registrations;
//        private readonly IContainerFactory _containerFactory;
//        private readonly IRegistrationFactory _registrationFactory;

//        internal ContainerBuilder(
//            CompositeRegistration registrations,
//            IContainerFactory containerFactory,
//            IRegistrationFactory registrationFactory)
//        {
//            _registrations = registrations;
//            _containerFactory = containerFactory;
//            _registrationFactory = registrationFactory;

//            _registrations.Add(_registrationFactory.MakeRegistrationForLifetimeScope());
//        }

//        public ContainerBuilder RegisterType<TService, TImpl>(Lifestyle lifestyle)
//            where TService : class
//            where TImpl : class, TService
//        {
//            EnsureWasNotBuilt();
//            AddRegistrationsFrom(lifestyle, _registrationFactory.MakeFactoryFor<TService, TImpl>());
//            return this;
//        }
//        public ContainerBuilder RegisterType<TService>(Func<ILifetimeScope, TService> instanceLookup, Lifestyle lifestyle)
//            where TService : class
//        {
//            EnsureWasNotBuilt();
//            AddRegistrationsFrom(lifestyle, _registrationFactory.MakeFactoryFor(instanceLookup));
//            return this;
//        }

//        public ContainerBuilder RegisterInstance<TInstance>(TInstance instance)
//             where TInstance : class
//        {
//            EnsureWasNotBuilt();
//            _registrations.Add(_registrationFactory.MakeRegistrationForInstance(instance));
//            return this;
//        }

//        protected override IContainer OnBuild()
//        {
//            return _containerFactory.Make(_registrations);
//        }

//        private void AddRegistrationsFrom(Lifestyle lifestyle, IRegistrationPerLifestyleFactory factory)
//        {
//            _registrations.AddRange(lifestyle.MakeRegistrations(factory));
//        }
//    }
//}
