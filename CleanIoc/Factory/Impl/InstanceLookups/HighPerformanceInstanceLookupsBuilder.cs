using System;
using CleanIoc.Core;
using CleanIoc.Factory.Impl.InstanceLookups.Base;

namespace CleanIoc.Factory.Impl.InstanceLookups
{
    class HighPerformanceInstanceLookupsBuilder : LazyInstanceLookupBuilder
    {
        public static readonly HighPerformanceInstanceLookupsBuilder Instance = new HighPerformanceInstanceLookupsBuilder();
        private static readonly InstanceLookupFinder Finder = new InstanceLookupFinder();

        private HighPerformanceInstanceLookupsBuilder() { }

        protected override void AddLazy(Type serviceType, Lazy<InstanceLookup<object>> lazy)
        {
            Finder.Add(serviceType, lazy);
        }

        protected override IInstanceLookupFinder OnBuild()
        {
            return Finder;
        }

        private static class InstanceLookupCache<TService> where TService : class
        {
            public static readonly InstanceLookup<TService> Value;

            static InstanceLookupCache()
            {
                Value = Finder[typeof(TService)].Value as InstanceLookup<TService>;
            }
        }

        private class InstanceLookupFinder : DictionaryInstanceLookupFinder, IInstanceLookupFinder
        {
            public InstanceLookup<TService> Find<TService>() where TService : class
            {
                return InstanceLookupCache<TService>.Value;
            }
        }
    }
}
