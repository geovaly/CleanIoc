using System;
using System.Linq;
using CleanIoc.Expressions;
using CleanIoc.Registrations;
using CleanIoc.Registrations.Impl.TagRegistrations.Others;
using CleanIoc.Registrations.Impl.TagRegistrations.Reflection;

namespace CleanIoc.Builder.Impl
{
    class RegistrationFactory : IRegistrationFactory
    {
        private readonly InstanceFactory _instanceFactory;
        private readonly InstanceExpressionFactory _expressionFactory;
        private readonly InstanceExpressionCompiler _expressionCompiler;

        public RegistrationFactory(
            InstanceFactory instanceFactory,
            InstanceExpressionFactory expressionFactory,
            InstanceExpressionCompiler expressionCompiler)
        {
            _instanceFactory = instanceFactory;
            _expressionFactory = expressionFactory;
            _expressionCompiler = expressionCompiler;
        }

        public IRegistrationFactoryPerLifestyle MakeFactoryFor<TService, TImpl>()
            where TService : class
            where TImpl : class, TService
        {
            return new ReflectionRegistrationFactoryPerLifestyle<TService, TImpl>(_instanceFactory, _expressionFactory, _expressionCompiler);
        }

        public IRegistrationFactoryPerLifestyle MakeFactoryFor<TService>(Func<ILifetimeScope, TService> instanceLookup) where TService : class
        {
            return new DelegateRegistrationFactoryPerLifestyle<TService>(instanceLookup);
        }

        public IRegistration MakeForLifetimeScope()
        {
            return new LifetimeScopeRegistration();
        }

        public IRegistration MakeForInstance<TInstance>(TInstance instance) where TInstance : class
        {
            return new InstanceRegistration<TInstance>(instance);
        }

        public IRegistration MakeForStateScope<TState>(object[] tags) where TState : class
        {
            return new CompositeRegistration(tags.Select(tag =>
                 new ScopeStateRegistration<TState>(tag)));
        }
    }
}
