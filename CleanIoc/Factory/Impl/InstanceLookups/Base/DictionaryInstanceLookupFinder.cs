using System;
using System.Collections.Generic;
using CleanIoc.Core;

namespace CleanIoc.Factory.Impl.InstanceLookups.Base
{
    class DictionaryInstanceLookupFinder : Dictionary<Type, Lazy<InstanceLookup<object>>>
    {
        public InstanceLookup<object> Find(Type serviceType)
        {
            Lazy<InstanceLookup<object>> lazyResult;
            if (!TryGetValue(serviceType, out lazyResult))
                throw new BadConfigurationException(string.Format(ExceptionMessages.TypeNotRegistered, serviceType));

            return lazyResult.Value;
        }
    }
}
