using System.Linq;
using CleanIoc.Builder.Lifestyles;
using CleanIoc.Expressions;
using CleanIoc.Registrations;
using CleanIoc.Registrations.Impl.TagRegistrations.Reflection;

namespace CleanIoc.Builder.Impl
{
    class ReflectionRegistrationFactoryPerLifestyle<TService, TImpl> : IRegistrationFactoryPerLifestyle
        where TService : class
        where TImpl : class, TService
    {
        private readonly InstanceFactory _instanceFactory;
        private readonly InstanceExpressionFactory _expressionFactory;
        private readonly InstanceExpressionCompiler _expressionCompiler;

        public ReflectionRegistrationFactoryPerLifestyle(
            InstanceFactory instanceFactory,
            InstanceExpressionFactory expressionFactory,
            InstanceExpressionCompiler expressionCompiler)
        {
            _instanceFactory = instanceFactory;
            _expressionFactory = expressionFactory;
            _expressionCompiler = expressionCompiler;
        }

        public IRegistration Make(SingletonLifestyle lifestyle)
        {
            return new SingletonRegistration<TService, TImpl>(_instanceFactory);
        }

        public IRegistration Make(TransientLifestyle lifestyle)
        {
            return new TransientRegistration<TService, TImpl>(_expressionFactory, _expressionCompiler);
        }

        public IRegistration Make(SingletonPerScopeLifestyle lifestyle)
        {
            return new CompositeRegistration(lifestyle.Tags.Select(tag =>
              new SingletonPerScopeRegistration<TService, TImpl>(tag, _expressionFactory, _expressionCompiler)));
        }

        public IRegistration Make(TransientPerScopeLifestyle lifestyle)
        {
            return new CompositeRegistration(lifestyle.Tags.Select(tag =>
              new TransientPerScopeRegistration<TService, TImpl>(tag, _expressionFactory, _expressionCompiler)));
        }
    }
}
