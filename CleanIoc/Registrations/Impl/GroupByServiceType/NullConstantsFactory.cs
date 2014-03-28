using System;
using System.Linq.Expressions;

namespace CleanIoc.Registrations.Impl.GroupByServiceType
{
    class NullConstantsFactory : IConstantFactory
    {
        public static readonly IConstantFactory Instance = new NullConstantsFactory();

        private NullConstantsFactory() { }

        private readonly NullConstant _nullConstant = new NullConstant();

        public IConstant MakeConstant<TValue>(TValue value) where TValue : class
        {
            return _nullConstant;
        }

        private class NullConstant : IConstant
        {
            public Expression ValueExpression { get { throw new InvalidOperationException(); } }
        }
    }
}
