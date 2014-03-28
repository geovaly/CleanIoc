using System;
using CleanIoc.Core;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate
{
    class SingletonPerScopeRegistration<TService> : BaseSingletonPerScopeRegistration<TService>
        where TService : class
    {
        private readonly DisposableInstanceLookup<TService> _instanceLookup;


        public SingletonPerScopeRegistration(object tag, Func<ILifetimeScope, TService> instanceLookup)
            : base(tag)
        {
            _instanceLookup = new DisposableInstanceLookup<TService>(instanceLookup);
        }

        protected override InstanceLookup<TService> MakeInstanceLookupThatMakesANewInstance()
        {
            return _instanceLookup.MakeDisposableInstanceLookup();
        }
    }
}
