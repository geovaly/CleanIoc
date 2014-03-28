using System;
using CleanIoc.Core;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate
{
    class TransientRegistration<TService> : BaseTransientDelegateRegistration<TService>
        where TService : class
    {
        private readonly DisposableInstanceLookup<TService> _instanceLookup;

        public TransientRegistration(Func<ILifetimeScope, TService> instanceLookup)
            : base(instanceLookup)
        {
            _instanceLookup = new DisposableInstanceLookup<TService>(instanceLookup);
        }

        public override InstanceLookup<TService> MakeInstanceLookup()
        {
            return _instanceLookup.MakeDisposableInstanceLookup();
        }

    }

}
