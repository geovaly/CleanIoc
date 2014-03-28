using System;
using CleanIoc.Core;
using CleanIoc.Factory.Impl.InstanceLookups.Base;

namespace CleanIoc.Factory.Impl.InstanceLookups
{

    class DictionaryInstanceLookupsBuilder : LazyInstanceLookupBuilder
    {
        private readonly InstanceLookupFinder _finder = new InstanceLookupFinder();

        protected override void AddLazy(Type serviceType, Lazy<InstanceLookup<object>> lazy)
        {
            _finder.Add(serviceType, lazy);
        }

        protected override IInstanceLookupFinder OnBuild()
        {
            return _finder;
        }

        private class InstanceLookupFinder : DictionaryInstanceLookupFinder, IInstanceLookupFinder
        {
            public InstanceLookup<TService> Find<TService>() where TService : class
            {
                return Find(typeof(TService)) as InstanceLookup<TService>;
            }
        }
    }
}
