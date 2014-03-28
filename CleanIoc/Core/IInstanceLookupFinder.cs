using System;

namespace CleanIoc.Core
{
    interface IInstanceLookupFinder
    {
        InstanceLookup<TService> Find<TService>() where TService : class;

        InstanceLookup<object> Find(Type serviceType);
    }
}