using System;
using System.Collections.Generic;
using CleanIoc.Registrations;
using CleanIoc.Utility;

namespace CleanIoc.Expressions.Impl.Builders
{
    class FuncInstanceExpressionFinderFactory : Dictionary<Type, Func<InstanceExpression>>, IInstanceExpressionsBuilder
    {
        public void Visit<TService>(IRegistration<TService> registration) where TService : class
        {
            Add(typeof(TService), registration.MakeInstanceExpression);
        }

        public IInstanceExpressionFinder Build()
        {
            return new InstanceExpressionFinder(this);
        }

        private class InstanceExpressionFinder : SortedList<Type, Func<InstanceExpression>>, IInstanceExpressionFinder
        {
            public InstanceExpressionFinder(FuncInstanceExpressionFinderFactory dictionary)
                : base(dictionary, new TypeComparer())
            {
            }

            public InstanceExpression FindExpressionFor(Type serviceType)
            {
                Func<InstanceExpression> factory;

                if (!TryGetValue(serviceType, out factory))
                    throw new BadConfigurationException(
                        string.Format(ExceptionMessages.TypeNotRegistered, serviceType));

                return factory.Invoke();
            }
        }
    }
}
