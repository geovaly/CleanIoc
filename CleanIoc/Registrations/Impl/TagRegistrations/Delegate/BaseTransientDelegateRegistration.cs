using System;
using System.Linq.Expressions;
using CleanIoc.Expressions;
using CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate
{
    abstract class BaseTransientDelegateRegistration<TService> : BaseTransientRegistration<TService> where TService : class
    {
        protected Func<ILifetimeScope, TService> InstanceLookup;
        private IConstant _instanceLookupConst;

        protected BaseTransientDelegateRegistration(Func<ILifetimeScope, TService> instanceLookup)
        {
            InstanceLookup = instanceLookup;
        }

        protected BaseTransientDelegateRegistration(object tag, Func<ILifetimeScope, TService> instanceLookup)
            : base(tag)
        {
            InstanceLookup = instanceLookup;
        }

        public override void OnMakingConstants(IConstantFactory factory)
        {
            _instanceLookupConst = factory.MakeConstant(InstanceLookup);
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            var instance = InvokeUserInstanceLookupExpression();

            return new InstanceExpression(ServiceType.CanBeDisposable()
                ? AddInstanceForDisposal(instance)
                : instance);
        }

        private Expression InvokeUserInstanceLookupExpression()
        {
            return Expression.Call(
                _instanceLookupConst.ValueExpression,
                "Invoke",
                null,
                Parameters.CurrentScope);
        }

        private static Expression AddInstanceForDisposal(Expression instance)
        {
            return Expression.Call(
                typeof(LifetimeScopeExtensions),
                "TryAddInstanceForDisposalAndReturnsTheInstance",
                new[] { ServiceType },
                Parameters.CurrentScope,
                instance);
        }
    }
}
