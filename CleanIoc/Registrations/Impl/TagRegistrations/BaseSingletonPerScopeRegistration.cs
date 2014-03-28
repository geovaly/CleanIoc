using System.Linq.Expressions;
using System.Threading;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    abstract class BaseSingletonPerScopeRegistration<TService> : InitSingletonRegistration<TService>
        where TService : class
    {
        private InstanceLookup<TService> _instanceFactory;
        private IConstant _instanceLookupConst;
        private readonly object _syncRoot = new object();

        protected BaseSingletonPerScopeRegistration(object tag)
            : base(tag)
        {
        }

        protected abstract InstanceLookup<TService> MakeInstanceLookupThatMakesANewInstance();

        public override void OnMakingConstants(IConstantFactory factory)
        {
            _instanceLookupConst = factory.MakeConstant<InstanceLookup<TService>>(GetInstanceSync);
        }

        public override InstanceLookup<TService> MakeInstanceLookup()
        {
            LoadInstanceFactory();
            return GetInstance;
        }

        private TService GetInstance(LifetimeScope scope)
        {
            LifetimeScope tagScope = scope.FindParentWith(TagIndex);
            object instance = tagScope.Singletons[SingletonIndex];

            return instance != null
                ? instance as TService
                : GetInstanceSync(tagScope);
        }

        private TService GetInstanceSync(LifetimeScope tagScope)
        {
            lock (tagScope.Singletons)
            {
                object instance = tagScope.Singletons[SingletonIndex];

                return instance != null
                    ? instance as TService
                    : FreshInstance(tagScope);
            }
        }

        private TService FreshInstance(LifetimeScope tagScope)
        {
            TService freshInstance = _instanceFactory(tagScope);
            Thread.MemoryBarrier();
            tagScope.Singletons[SingletonIndex] = freshInstance;
            return freshInstance;
        }

        private void LoadInstanceFactory()
        {
            if (_instanceFactory != null)
                return;

            lock (_syncRoot)
            {
                if (_instanceFactory != null)
                    return;

                _instanceFactory = MakeInstanceLookupThatMakesANewInstance();
            }
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            LoadInstanceFactory();

            VariableExpression scopeVar = MakeScopeVariable();
            VariableExpression instanceVar = MakeInstanceVariable(scopeVar.Var);

            return new InstanceExpression(instanceVar.Var).AddVariables(scopeVar, instanceVar);
        }


        private VariableExpression MakeInstanceVariable(ParameterExpression scope)
        {
            var instance = Expression.Variable(ServiceType);
            var initInstance = AssignInstance(instance, scope);
            return new VariableExpression(instance, initInstance);
        }

        private Expression AssignInstance(ParameterExpression instance, ParameterExpression scope)
        {
            return Expression.Assign(instance, CoalesceInstance(scope));
        }

        private Expression CoalesceInstance(ParameterExpression scope)
        {
            return Expression.Coalesce(
                GetSingletonFromScopeExpression(scope),
                MakeANewInstance(scope));
        }

        private Expression MakeANewInstance(ParameterExpression scope)
        {
            return Expression.Call(
                _instanceLookupConst.ValueExpression,
                "Invoke",
                null,
                scope);
        }
    }
}
