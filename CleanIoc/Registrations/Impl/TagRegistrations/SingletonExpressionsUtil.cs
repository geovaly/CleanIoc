using System;
using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    static class SingletonExpressionsUtil
    {
        private static readonly Expression ContainerSingletonsField =
            Expression.Field(
                Parameters.CurrentScope,
                typeof(LifetimeScope),
                "RootSingletons");

        public static Expression GetSingletonFromContainer(Type serviceType, int index)
        {
            return TypeAsService(serviceType, GetSingletonFromContainerAsObject(index));
        }

        public static Expression GetSingletonFromScope(Type serviceType, int index, ParameterExpression scope)
        {
            return TypeAsService(serviceType, GetSingletonFromScopeAsObject(index, scope));
        }

        private static Expression TypeAsService(Type serviceType, Expression expression)
        {
            return Expression.TypeAs(expression, serviceType);
        }

        private static Expression GetSingletonFromContainerAsObject(int index)
        {
            return Expression.ArrayIndex(
                ContainerSingletonsField,
                Expression.Constant(index));
        }

        private static Expression GetSingletonFromScopeAsObject(int index, ParameterExpression scopeVar)
        {
            return Expression.ArrayIndex(
                SingletonsFieldFrom(scopeVar),
                Expression.Constant(index));
        }

        private static MemberExpression SingletonsFieldFrom(ParameterExpression scopeVar)
        {
            return Expression.Field(
                 scopeVar,
                 typeof(LifetimeScope),
                 "Singletons");
        }
    }
}
