using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Reflection
{
    class TransientPerScopeRegistration<TService, TImpl> : BaseTransientRegistration<TService>
        where TService : class
        where TImpl : class, TService
    {
        private readonly InstanceExpressionFactory _expressionFactory;
        private readonly InstanceExpressionCompiler _expressionCompiler;

        public TransientPerScopeRegistration(object tag, InstanceExpressionFactory expressionFactory, InstanceExpressionCompiler expressionCompiler)
            : base(tag)
        {
            _expressionFactory = expressionFactory;
            _expressionCompiler = expressionCompiler;
        }


        public override InstanceLookup<TService> MakeInstanceLookup()
        {
            return _expressionCompiler.Compile<TImpl>(MakeInstanceExpression());
        }

        public override InstanceExpression MakeInstanceExpression()
        {
            return _expressionFactory.MakeInstanceExpressionThatMakesANewInstance(typeof(TImpl))
                .AddVariables(MakeScopeVariable());
        }

    }
}
