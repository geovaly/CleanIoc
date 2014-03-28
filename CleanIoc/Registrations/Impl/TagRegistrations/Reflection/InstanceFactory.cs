using System;
using System.Linq;
using CleanIoc.Core;
using CleanIoc.Policy;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Reflection
{
    class InstanceFactory 
    {
        private readonly IConstructorSelectorPolicy _constructorSelectorPolicy;

        public InstanceFactory(IConstructorSelectorPolicy constructorSelectorPolicy)
        {
            _constructorSelectorPolicy = constructorSelectorPolicy;
        }

        public TInstance MakeInstance<TInstance>(LifetimeScope scope) where TInstance : class
        {
            var constructor = _constructorSelectorPolicy.SelectConstructor(typeof(TInstance));

            var parameters = constructor.GetParameters()
                .Select(p => scope.Resolve(p.ParameterType))
                .ToArray();
            
           var instance = constructor.Invoke(parameters) as TInstance;
            AddInstanceForDisposal(scope, instance);
            return instance;
        }

        private static void AddInstanceForDisposal(LifetimeScope scope, object instance) 
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
                scope.AddInstanceForDisposalAndReturnsTheInstance(disposable);
        }
    }
}
