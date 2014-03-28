using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Reflection
{
    class SingletonPerScopeRegistration<TService, TImpl> : BaseSingletonPerScopeRegistration<TService>
        where TService : class
        where TImpl : class, TService
    {
        private readonly InstanceExpressionFactory _expressionFactory;
        private readonly InstanceExpressionCompiler _expressionCompiler;

        public SingletonPerScopeRegistration(object tag,
            InstanceExpressionFactory expressionFactory,
            InstanceExpressionCompiler expressionCompiler)
            : base(tag)
        {
            _expressionFactory = expressionFactory;
            _expressionCompiler = expressionCompiler;
        }

        protected override InstanceLookup<TService> MakeInstanceLookupThatMakesANewInstance()
        {
            return _expressionCompiler.Compile<TImpl>(
                _expressionFactory.MakeInstanceExpressionThatMakesANewInstance(typeof(TImpl)));
        }

    }
}
