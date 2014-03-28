using System;
using FakeItEasy;
using CleanIoc.Builder;
using CleanIoc.Registrations;

namespace CleanIoc.Tests.UnitTests.TestDoubles
{
    static class StubRegistrationFactory
    {
        public static IRegistrationFactory ReturnFactoryForDelegate<TService>(
          IRegistrationFactoryPerLifestyle factoryResult,
          Func<ILifetimeScope, TService> instanceLookup)
          where TService : class
        {
            var registrationFactory = A.Fake<IRegistrationFactory>();

            A.CallTo(() => registrationFactory.MakeFactoryFor(instanceLookup))
                .Returns(factoryResult);

            return registrationFactory;
        }

        public static IRegistrationFactory ReturnFactoryForReflection<TService, TImpl>(IRegistrationFactoryPerLifestyle factoryResult)
            where TService : class
            where TImpl : class, TService
        {
            var registrationFactory = A.Fake<IRegistrationFactory>();

            A.CallTo(() => registrationFactory.MakeFactoryFor<TService, TImpl>())
                .Returns(factoryResult);

            return registrationFactory;
        }

        public static IRegistrationFactory ReturnRegistrationForInstance<TService>(
            IRegistration registrationResult,
            TService instance)
            where TService : class
        {
            var registrationFactory = A.Fake<IRegistrationFactory>();

            A.CallTo(() => registrationFactory.MakeForInstance(instance))
                .Returns(registrationResult);

            return registrationFactory;
        }

        public static IRegistrationFactory ReturnRegistrationForLifetimeScope(IRegistration registration)
        {
            var registrationFactory = A.Fake<IRegistrationFactory>();

            A.CallTo(() => registrationFactory.MakeForLifetimeScope())
                .Returns(registration);

            return registrationFactory;
        }

        public static IRegistrationFactory ReturnRegistrationForState<TState>(IRegistration registration, TState state, object[] tags)
            where TState : class
        {
            var registrationFactory = A.Fake<IRegistrationFactory>();

            A.CallTo(() => registrationFactory.MakeForStateScope<TState>(A<object[]>.That.IsSameSequenceAs(tags)))
                .Returns(registration);

            return registrationFactory;
        }
    }
}
