using System;
using System.Linq;
using CleanIoc.Builder.Lifestyles;
using CleanIoc.Registrations;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate;

namespace CleanIoc.Builder.Impl
{
    class DelegateRegistrationFactoryPerLifestyle<TService> : IRegistrationFactoryPerLifestyle
        where TService : class
    {
        private readonly Func<ILifetimeScope, TService> _instanceLookup;

        public DelegateRegistrationFactoryPerLifestyle(Func<ILifetimeScope, TService> instanceLookup)
        {
            _instanceLookup = instanceLookup;
        }

        public IRegistration Make(SingletonLifestyle lifestyle)
        {
            return new SingletonRegistration<TService>(_instanceLookup);
        }

        public IRegistration Make(TransientLifestyle lifestyle)
        {
            return new TransientRegistration<TService>(_instanceLookup);
        }

        public IRegistration Make(SingletonPerScopeLifestyle lifestyle)
        {
            return new CompositeRegistration(lifestyle.Tags.Select(tag =>
                 new SingletonPerScopeRegistration<TService>(tag, _instanceLookup)));
        }

        public IRegistration Make(TransientPerScopeLifestyle lifestyle)
        {
            return new CompositeRegistration(lifestyle.Tags.Select(tag =>
                new TransientPerScopeRegistration<TService>(tag, _instanceLookup)));
        }
    }
}
