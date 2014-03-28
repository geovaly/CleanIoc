using System.Linq.Expressions;

namespace CleanIoc.Registrations
{
    interface IConstant
    {
        Expression ValueExpression { get; }
    }
}
