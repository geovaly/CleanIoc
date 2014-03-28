using System;
using CleanIoc.Registrations;

namespace CleanIoc.Builder
{
    interface IRegistrationFactory
    {
        IRegistrationFactoryPerLifestyle MakeFactoryFor<TService, TImpl>()
            where TService : class
            where TImpl : class, TService;

        IRegistrationFactoryPerLifestyle MakeFactoryFor<TService>(
            Func<ILifetimeScope, TService> instanceLookup)
            where TService : class;

        IRegistration MakeForLifetimeScope();

        IRegistration MakeForInstance<TInstance>(TInstance instance)
            where TInstance : class;

        IRegistration MakeForStateScope<TState>(object[] tags)
            where TState : class;
    }
}
