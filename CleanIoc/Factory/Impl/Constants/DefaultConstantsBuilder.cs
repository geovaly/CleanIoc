using System;
using System.Linq.Expressions;
using CleanIoc.Registrations;

namespace CleanIoc.Factory.Impl.Constants
{
    class DefaultConstantsBuilder : IConstantsBuilder
    {
        public IConstant MakeConstant<TValue>(TValue value) where TValue : class
        {
            return new Constant(typeof(TValue), value);
        }

        public object[] Build()
        {
            return new object[0];
        }

        private class Constant : IConstant
        {
            private readonly Type _type;
            private readonly object _value;

            public Constant(Type type, object value)
            {
                _type = type;
                _value = value;
            }


            public Expression ValueExpression
            {
                get
                {
                    return Expression.Constant(_value, _type);
                }
            }
        }
    }
}
