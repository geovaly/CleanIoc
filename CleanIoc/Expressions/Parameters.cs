using System.Linq.Expressions;
using CleanIoc.Core;

namespace CleanIoc.Expressions
{
    public static class Parameters
    {
        public static readonly ParameterExpression CurrentScope = Expression.Parameter(typeof(LifetimeScope), "currentScope");
    }
}
