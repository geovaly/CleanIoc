using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CleanIoc.Utility
{
    static class ReflectionHelpers
    {
        public static string GetMethodName<TDeclaring>(Expression<Action<TDeclaring>> methodCallExpression)
        {
            return GetMethod(methodCallExpression).Name;
        }

        public static MethodInfo GetMethod<TDeclaring>(Expression<Action<TDeclaring>> methodCallExpression)
        {
            return ((MethodCallExpression)methodCallExpression.Body).Method;
        }

        public static string GetPropertyName<TDeclaring>(Expression<Func<TDeclaring, object>> propertyLambda)
        {
            return GetProperty(propertyLambda).Name;
        }

        public static MemberInfo GetProperty<TDeclaring>(Expression<Func<TDeclaring, object>> propertyLambda)
        {
            return ((MemberExpression)propertyLambda.Body).Member;
        }
    }
}
