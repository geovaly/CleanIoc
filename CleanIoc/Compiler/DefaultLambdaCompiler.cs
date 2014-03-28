using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Compiler
{
    class DefaultLambdaCompiler : ILambdaCompiler
    {
        public InstanceLookup<TService> Compile<TService>(Expression<InstanceLookup<TService>> expression) 
            where TService : class
        {
            return expression.Compile();
        }
    }
}
