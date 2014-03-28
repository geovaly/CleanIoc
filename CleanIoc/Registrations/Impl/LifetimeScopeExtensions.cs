using System;
using CleanIoc.Core;

namespace CleanIoc.Registrations.Impl
{
    static class LifetimeScopeExtensions
    {
        public static LifetimeScope FindParentWith(this LifetimeScope scope, int tagIndex)
        {
            while (scope.TagIndex != tagIndex)
            {
                scope = scope.Parent;
                if (scope == null)
                    throw new BadConfigurationException();
            }

            return scope;
        }

        public static TService AddInstanceForDisposalAndReturnsTheInstance<TService>(this LifetimeScope scope, TService instance)
            where TService : class, IDisposable
        {
            scope.AddInstanceForDisposal(instance);
            return instance;
        }

        public static TService TryAddInstanceForDisposalAndReturnsTheInstance<TService>(this LifetimeScope scope, TService instance)
            where TService : class
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
                scope.AddInstanceForDisposal(disposable);

            return instance;
        }
    }
}
