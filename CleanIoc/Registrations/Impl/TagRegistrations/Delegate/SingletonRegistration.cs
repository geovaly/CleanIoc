using System;
using CleanIoc.Core;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate
{
    class SingletonRegistration<TService> : BaseSingletonRegistration<TService>
        where TService : class
    {
        private readonly DisposableInstanceLookup<TService> _instanceLookup;

        public SingletonRegistration(Func<ILifetimeScope, TService> instanceLookup)
        {
            _instanceLookup = new DisposableInstanceLookup<TService>(instanceLookup);
        }

        protected override TService MakeANewInstance(LifetimeScope scope)
        {
            return _instanceLookup.GetDisposableInstance(scope);
        }
    }
}
