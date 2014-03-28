using CleanIoc.Registrations;

namespace CleanIoc.Expressions.Impl
{
    interface IInstanceExpressionsBuilder : IRegistrationVisitor
    {
        IInstanceExpressionFinder Build();
    }
}
