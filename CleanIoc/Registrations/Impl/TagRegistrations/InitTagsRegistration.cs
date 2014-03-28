using System;
using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;
using CleanIoc.Registrations.Impl.GroupByServiceType;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    abstract class InitTagsRegistration<TService> : ITagRegistration<TService> where TService : class
    {
        protected readonly object Tag;
        protected int TagIndex;

        protected Container Container { get; private set; }

        protected InitTagsRegistration()
            : this(Container.RootTag)
        {
        }

        protected InitTagsRegistration(object tag)
        {
            Tag = tag;
        }

        public void Accept(IRegistrationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void OnInitTags(ITagsInitializer initializer)
        {
            initializer.AddTagIfNotExists(Tag);
            TagIndex = initializer.GetIndexOfAddedTag(Tag);
        }

        public virtual void OnInitSingletons(ISingletonsInitializer initializer)
        {
        }

        public virtual void OnMakingConstants(IConstantFactory factory)
        {
        }

        public virtual void OnContainerWasBuilt(Container container)
        {
            Container = container;
        }

        protected VariableExpression MakeScopeVariable()
        {
            return ScopeExpressionsUtil.MakeScopeVariable(TagIndex);
        }

        public abstract InstanceExpression MakeInstanceExpression();

        public abstract InstanceLookup<TService> MakeInstanceLookup();

        public abstract bool IsSingleton { get; }

        int ITagRegistration<TService>.TagIndex
        {
            get { return TagIndex; }
        }

        protected static Type ServiceType { get { return typeof(TService); } }

    }
}
