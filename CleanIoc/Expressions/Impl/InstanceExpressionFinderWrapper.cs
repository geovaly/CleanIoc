using System;

namespace CleanIoc.Expressions.Impl
{
    class InstanceExpressionFinderWrapper : IInstanceExpressionFinder
    {
        private IInstanceExpressionFinder _finder;

        public void SetFinder(IInstanceExpressionFinder finder)
        {
            _finder = finder;
        }

        public InstanceExpression FindExpressionFor(Type serviceType)
        {
            if (_finder == null)
                throw new InvalidOperationException();

            return _finder.FindExpressionFor(serviceType);
        }
    }
}
