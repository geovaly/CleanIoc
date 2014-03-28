using System;
using System.Collections.Generic;
using CleanIoc.Registrations;
using CleanIoc.Utility;

namespace CleanIoc.Expressions.Impl.Builders
{
    class LazyInstanceExpressionsBuilder : Dictionary<Type, Lazy<InstanceExpression>>, IInstanceExpressionsBuilder
    {
        public void Visit<TService>(IRegistration<TService> registration) where TService : class
        {
            Add(typeof(TService), new Lazy<InstanceExpression>(registration.MakeInstanceExpression));
        }

        public IInstanceExpressionFinder Build()
        {
            return new InstanceExpressionFinder(this);
        }

        private class InstanceExpressionFinder : SortedList<Type, Lazy<InstanceExpression>>, IInstanceExpressionFinder
        {
            public InstanceExpressionFinder(LazyInstanceExpressionsBuilder dictionary)
                : base(dictionary, new TypeComparer())
            {
            }

            public InstanceExpression FindExpressionFor(Type serviceType)
            {
                Lazy<InstanceExpression> lazyResult;

                if (!TryGetValue(serviceType, out lazyResult))
                    throw new BadConfigurationException(
                        string.Format(ExceptionMessages.TypeNotRegistered, serviceType));

                return lazyResult.Value;
            }
        }
    }
}
