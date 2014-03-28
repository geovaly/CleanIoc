using System;
using CleanIoc.Core;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Others
{
    class InstanceRegistration<TService> : BaseSingletonRegistration<TService> where TService : class
    {
        private readonly TService _instance;

        public InstanceRegistration(TService instance)
        {
            _instance = instance;
        }

        public override void OnContainerWasBuilt(Container container)
        {
            base.OnContainerWasBuilt(container);
            LoadInstance();
        }

        protected override TService MakeANewInstance(LifetimeScope scope)
        {
            var disposable = _instance as IDisposable;
            if (disposable != null)
                Container.AddInstanceForDisposalAndReturnsTheInstance(disposable);

            return _instance;
        }
    }
}
