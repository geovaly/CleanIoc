using System;
using CleanIoc.Registrations;

namespace CleanIoc.Expressions.Impl
{
    class InstanceExpressionFinderFactory
    {
        private readonly Func<IInstanceExpressionsBuilder> _makeBuilder;

        public InstanceExpressionFinderFactory(Func<IInstanceExpressionsBuilder> makeBuilder)
        {
            _makeBuilder = makeBuilder;
        }

        public IInstanceExpressionFinder Make(IRegistration registration)
        {
            var builder = _makeBuilder.Invoke();
            registration.Accept(builder);
            return builder.Build();
        }
    }
}
