using System;
using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Others
{
    class ScopeStateRegistration<TState> : BaseTransientRegistration<TState>
        where TState : class
    {

        public ScopeStateRegistration(object tag)
            : base(tag)
        {
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            var scopeVar = MakeScopeVariable();

            return new InstanceExpression(
                 GetStateExpression(scopeVar))
                 .AddVariables(scopeVar);
        }

        private static Expression GetStateExpression(VariableExpression scopeVar)
        {
            return Expression.Convert(
                Expression.Field(scopeVar.Var, "State"),
                typeof(TState));
        }

        public override InstanceLookup<TState> MakeInstanceLookup()
        {
            return currentScope =>
            {
                var scope = currentScope.FindParentWith(TagIndex);
                return (TState)scope.State;
            };
        }
    }
}
