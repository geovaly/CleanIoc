using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    static class ScopeExpressionsUtil
    {
        public static ParameterExpression MakeScopeParameter(int tagIndex)
        {
            return Expression.Variable(typeof(LifetimeScope), "scope" + tagIndex);
        }

        public static VariableExpression MakeScopeVariable(int tagIndex)
        {
            var scope = MakeScopeParameter(tagIndex);
            return new VariableExpression(scope, Expression.Assign(scope, FindTagScope(tagIndex)));
        }

        private static MethodCallExpression FindTagScope(int tagIndex)
        {
            return Expression.Call(
                typeof(LifetimeScopeExtensions),
                "FindParentWith",
                null,
                Parameters.CurrentScope,
                Expression.Constant(tagIndex, typeof(int)));
        }
    }
}
