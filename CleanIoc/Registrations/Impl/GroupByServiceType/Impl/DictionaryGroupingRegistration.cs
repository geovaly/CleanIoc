using System;
using System.Collections.Generic;
using System.Linq;
using CleanIoc.Core;

namespace CleanIoc.Registrations.Impl.GroupByServiceType.Impl
{
    class DictionaryGroupingRegistration<TService> : GroupingRegistration<TService> where TService : class
    {
        private Dictionary<int, InstanceLookup<TService>> _instanceLookupsByTagIndex;

        public DictionaryGroupingRegistration(IEnumerable<ITagRegistration<TService>> registrations)
            : base(registrations)
        {
        }

        protected override TService GetInstance(LifetimeScope currentScope)
        {
            for (var scope = currentScope; scope != null; scope = scope.Parent)
            {
                InstanceLookup<TService> instanceLookup;
                if (_instanceLookupsByTagIndex.TryGetValue(scope.TagIndex, out instanceLookup))
                    return instanceLookup.Invoke(currentScope);
            }

            throw new InvalidOperationException();
        }

        protected override void OnLoadInstanceLookups(IReadOnlyCollection<ITagRegistration<TService>> registrations)
        {
            _instanceLookupsByTagIndex = registrations.ToDictionary(
                x => x.TagIndex,
                x => x.MakeInstanceLookup());
        }
    }
}
