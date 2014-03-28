using System.Linq.Expressions;
using CleanIoc.Core;

namespace CleanIoc.Expressions
{
    interface ILambdaCompiler
    {
        InstanceLookup<TService> Compile<TService>(Expression<InstanceLookup<TService>> expression)
            where TService : class;
    }
}
