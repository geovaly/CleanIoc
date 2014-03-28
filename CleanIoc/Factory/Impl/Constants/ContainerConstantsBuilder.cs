using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;
using CleanIoc.Registrations;
using CleanIoc.Utility;

namespace CleanIoc.Factory.Impl.Constants
{
    class ContainerConstantsBuilder : BaseBuilder<object[]>, IConstantsBuilder
    {
        private readonly List<object> _constants = new List<object>();

        public IConstant MakeConstant<TValue>(TValue value) where TValue : class
        {
            var nextIndex = _constants.Count;
            _constants.Add(value);
            return new Constant(typeof(TValue), nextIndex);
        }

        protected override object[] OnBuild()
        {
            return _constants.ToArray();
        }

        private class Constant : IConstant
        {
            private static readonly Expression GetConstantsField = 
                Expression.Field(
                     Parameters.CurrentScope,
                     typeof(LifetimeScope),
                     "Constants");

            private readonly Type _type;
            private readonly int _index;

            public Constant(Type type, int index)
            {
                _type = type;
                _index = index;
            }

            public Expression ValueExpression
            {
                get { return Expression.TypeAs(GetObjectConstant(), _type); }
            }

            private Expression GetObjectConstant()
            {
                return Expression.ArrayIndex(
                  GetConstantsField,
                  Expression.Constant(_index, typeof(int)));
            }
        }
    }
}
