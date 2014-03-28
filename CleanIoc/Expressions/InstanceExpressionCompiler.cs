using System.Linq;
using System.Linq.Expressions;
using CleanIoc.Core;

namespace CleanIoc.Expressions
{
    class InstanceExpressionCompiler 
    {
        private readonly ILambdaCompiler _lambdaCompiler;

        public InstanceExpressionCompiler(ILambdaCompiler lambdaCompiler)
        {
            _lambdaCompiler = lambdaCompiler;
        }

        public InstanceLookup<TService> Compile<TService>(InstanceExpression expression)
            where TService : class
        {
            return _lambdaCompiler.Compile(MakeLambda<TService>(expression));
        }

        private static Expression<InstanceLookup<TService>> MakeLambda<TService>(InstanceExpression expression)
            where TService : class
        {
            return Expression.Lambda<InstanceLookup<TService>>(LambdaBody(expression), Parameters.CurrentScope);
        }

        private static Expression LambdaBody(InstanceExpression expression)
        {
            return expression.Variables.Any()
                ? BlockExpression(expression)
                : expression.Instance;
        }

        private static BlockExpression BlockExpression(InstanceExpression expression)
        {
            return Expression.Block(
                expression.Variables.Select(s => s.Var),
                expression.Variables.Select(s => s.InitVar)
                    .Concat(new[] { expression.Instance }));
        }

    }
}
