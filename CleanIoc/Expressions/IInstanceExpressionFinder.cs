using System;

namespace CleanIoc.Expressions
{
    interface IInstanceExpressionFinder
    {
        InstanceExpression FindExpressionFor(Type serviceType);
    }
}
