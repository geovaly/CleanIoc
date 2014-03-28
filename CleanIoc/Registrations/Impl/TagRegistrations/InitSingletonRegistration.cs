using System.Linq.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    abstract class InitSingletonRegistration<TService> : InitTagsRegistration<TService>
        where TService : class
    {
        protected int SingletonIndex;

        protected InitSingletonRegistration()
        {
        }

        protected InitSingletonRegistration(object tag)
            : base(tag)
        {
        }

        public override void OnInitSingletons(ISingletonsInitializer initializer)
        {
            initializer.AddSingletonFor(Tag);
            SingletonIndex = initializer.GetIndexOfLastAddedSingletonFor(Tag);
        }

        protected Expression GetSingletonFromContainerExpression()
        {
            return SingletonExpressionsUtil.GetSingletonFromContainer(typeof(TService), SingletonIndex);
        }

        protected Expression GetSingletonFromScopeExpression(ParameterExpression scope)
        {
            return SingletonExpressionsUtil.GetSingletonFromScope(typeof(TService), SingletonIndex, scope);
        }

        public override bool IsSingleton
        {
            get { return true; }
        }
    }
}
