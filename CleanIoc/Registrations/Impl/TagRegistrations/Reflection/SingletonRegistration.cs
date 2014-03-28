using CleanIoc.Core;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Reflection
{
    class SingletonRegistration<TService, TImpl> : BaseSingletonRegistration<TService>
        where TService : class
        where TImpl : class, TService
    {
        private readonly InstanceFactory _instanceFactory;

        public SingletonRegistration(InstanceFactory instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        protected override TService MakeANewInstance(LifetimeScope scope)
        {
            return _instanceFactory.MakeInstance<TImpl>(scope);
        }

    }
}
