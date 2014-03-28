using System;
using CleanIoc.Core;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util
{
    class DisposableInstanceLookup<TService> where TService : class
    {
        private readonly Func<ILifetimeScope, TService> _instanceLookup;

        public DisposableInstanceLookup(Func<ILifetimeScope, TService> instanceLookup)
        {
            _instanceLookup = instanceLookup;
        }

        public InstanceLookup<TService> MakeDisposableInstanceLookup()
        {
            if (typeof(TService).CanBeDisposable())
                return GetDisposableInstance;
            else
                return _instanceLookup.Invoke;
        }

        public TService GetDisposableInstance(LifetimeScope scope)
        {
            var instance = _instanceLookup.Invoke(scope);
            var disposable = instance as IDisposable;
            if (disposable != null)
                scope.AddInstanceForDisposalAndReturnsTheInstance(disposable);

            return instance;
        }
    }
}
