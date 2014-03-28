using System;

namespace CleanIoc.Core
{
    class LifetimeScope : Disposer, ILifetimeScope
    {
        public static readonly object AnonymousTag = "anonymous";

        private readonly IInstanceLookupFinder _instanceLookupFinder;
        private readonly ITagDetailsFinder _tagDetailsFinder;
        private readonly object _tag;

        public readonly int TagIndex;
        public readonly LifetimeScope Parent;
        public readonly object[] Singletons;
        public readonly object[] RootSingletons;
        public readonly object[] Constants;
        public readonly object State;

        protected LifetimeScope(
            object tag,
            object state,
            LifetimeScope parent,
            object[] constants,
            IInstanceLookupFinder instanceLookupFinder,
            ITagDetailsFinder tagDetailsFinder)
        {
            _tag = tag;
            State = state;
            Parent = parent;
            Constants = constants;
            _instanceLookupFinder = instanceLookupFinder;
            _tagDetailsFinder = tagDetailsFinder;
            var tagDetails = _tagDetailsFinder.Find(tag);
            TagIndex = tagDetails.TagIndex;
            Singletons = new object[tagDetails.SingletonsCount];
            RootSingletons = Parent == null ? Singletons : Parent.RootSingletons;
        }

        public TService Resolve<TService>() where TService : class
        {
            CheckNotDisposed();
            return _instanceLookupFinder.Find<TService>().Invoke(this);
        }

        public object Resolve(Type serviceType)
        {
            CheckNotDisposed();
            return _instanceLookupFinder.Find(serviceType).Invoke(this);
        }

        public ILifetimeScope BeginScope()
        {
            return BeginScope(AnonymousTag);
        }

        public ILifetimeScope BeginScope(object tag)
        {
            return BeginScope(tag, null);
        }

        public ILifetimeScope BeginScope(object tag, object state)
        {
            CheckNotDisposed();
            return new LifetimeScope(
                tag,
                state,
                this,
                Constants,
                _instanceLookupFinder,
                _tagDetailsFinder);
        }

        ILifetimeScope ILifetimeScope.Parent
        {
            get { return Parent; }
        }

        object ILifetimeScope.State
        {
            get { return State; }
        }

        object ILifetimeScope.Tag
        {
            get { return _tag; }
        }
    }
}
