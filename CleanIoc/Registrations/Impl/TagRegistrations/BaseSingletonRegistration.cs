using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    abstract class BaseSingletonRegistration<TService> : InitSingletonRegistration<TService>
        where TService : class
    {
        private readonly object _syncRoot = new object();

        protected abstract TService MakeANewInstance(LifetimeScope scope);

        public override InstanceLookup<TService> MakeInstanceLookup()
        {
            LoadInstance();
            var instance = Container.Singletons[SingletonIndex] as TService;
            return scope => instance;
        }

        protected void LoadInstance()
        {
            if (Container.Singletons[SingletonIndex] != null)
                return;

            lock (_syncRoot)
            {
                if (Container.Singletons[SingletonIndex] != null)
                    return;

                Container.Singletons[SingletonIndex] = MakeANewInstance(Container);
            }
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            LoadInstance();
            VariableExpression instanceVar = MakeSingletonFromContainerVariable();
            return new InstanceExpression(instanceVar.Var).AddVariables(instanceVar);
        }

        private VariableExpression MakeSingletonFromContainerVariable()
        {
            var instance = Expression.Variable(ServiceType);
            var initInstance = Expression.Assign(instance, GetSingletonFromContainerExpression());
            return new VariableExpression(instance, initInstance);
        }
    }
}
