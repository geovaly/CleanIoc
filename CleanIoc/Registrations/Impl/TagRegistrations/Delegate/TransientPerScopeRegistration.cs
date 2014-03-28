using System;
using CleanIoc.Core;
using CleanIoc.Expressions;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate
{
    class TransientPerScopeRegistration<TService> : BaseTransientDelegateRegistration<TService>
        where TService : class
    {
        public TransientPerScopeRegistration(object tag, Func<ILifetimeScope, TService> instanceLookup)
            : base(tag, instanceLookup)
        {
        }

        public override InstanceLookup<TService> MakeInstanceLookup()
        {
            if (ServiceType.CanBeDisposable())
                return GetDisposableInstance;
            else
                return GetInstance;
        }

        private TService GetInstance(LifetimeScope scope)
        {
            scope.FindParentWith(TagIndex);
            return InstanceLookup.Invoke(scope);
        }

        private TService GetDisposableInstance(LifetimeScope scope)
        {
            scope.FindParentWith(TagIndex);
            var instance = InstanceLookup.Invoke(scope);
            var disposable = instance as IDisposable;
            if (disposable != null)
                scope.AddInstanceForDisposalAndReturnsTheInstance(disposable);

            return instance;
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            return base.MakeInstanceExpression().AddVariables(MakeScopeVariable());
        }

    }
}
