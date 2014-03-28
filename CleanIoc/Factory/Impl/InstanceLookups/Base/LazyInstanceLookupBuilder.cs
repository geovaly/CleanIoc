using System;
using CleanIoc.Core;
using CleanIoc.Registrations;
using CleanIoc.Utility;

namespace CleanIoc.Factory.Impl.InstanceLookups.Base
{
    abstract class LazyInstanceLookupBuilder : BaseBuilder<IInstanceLookupFinder>, IInstanceLookupsBuilder
    {
        public void Visit<TService>(IRegistration<TService> registration) where TService : class
        {
            EnsureWasNotBuilt();
            AddLazy(typeof(TService), new Lazy<InstanceLookup<object>>(registration.MakeInstanceLookup));
        }

        protected abstract void AddLazy(Type serviceType, Lazy<InstanceLookup<object>> lazy);
    }
}
