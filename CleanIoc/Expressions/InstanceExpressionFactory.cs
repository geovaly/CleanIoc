using System;
using System.Linq;
using System.Linq.Expressions;
using CleanIoc.Policy;
using CleanIoc.Registrations.Impl;

namespace CleanIoc.Expressions
{
    class InstanceExpressionFactory
    {
        private readonly IInstanceExpressionFinder _expressionFinder;
        private readonly IConstructorSelectorPolicy _constructorSelector;

        public InstanceExpressionFactory(
            IInstanceExpressionFinder expressionFinder,
            IConstructorSelectorPolicy constructorSelector)
        {
            _expressionFinder = expressionFinder;
            _constructorSelector = constructorSelector;
        }

        public InstanceExpression MakeInstanceExpressionThatMakesANewInstance(Type concreteType)
        {
            var constructor = _constructorSelector.SelectConstructor(concreteType);

            var parameters = constructor.GetParameters()
                .Select(x => _expressionFinder.FindExpressionFor(x.ParameterType))
                .ToList();

            Expression newInstance = Expression.New(constructor, parameters.Select(x => x.Instance));

            if (typeof(IDisposable).IsAssignableFrom(concreteType))
                newInstance = AddInstanceForDisposal(newInstance, concreteType, Parameters.CurrentScope);

            return new InstanceExpression(newInstance).AddVariablesFrom(parameters);
        }

        private Expression AddInstanceForDisposal(Expression instance, Type concreteType, ParameterExpression scopeVar)
        {
            return Expression.Call(
                typeof(LifetimeScopeExtensions),
                "AddInstanceForDisposalAndReturnsTheInstance",
                new[] { concreteType },
                scopeVar,
                instance);
        }
    }
}
