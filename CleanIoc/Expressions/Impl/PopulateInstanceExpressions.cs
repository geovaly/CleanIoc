using CleanIoc.Builder;
using CleanIoc.Registrations;

namespace CleanIoc.Expressions.Impl
{
    class PopulateInstanceExpressions : IContainerFactory
    {
        private readonly InstanceExpressionFinderWrapper _wrapper;
        private readonly InstanceExpressionFinderFactory _finderFactory;
        private readonly IContainerFactory _containerFactory;

        public PopulateInstanceExpressions(
            InstanceExpressionFinderWrapper wrapper,
            InstanceExpressionFinderFactory finderFactory, 
            IContainerFactory containerFactory)
        {
            _wrapper = wrapper;
            _finderFactory = finderFactory;
            _containerFactory = containerFactory;
        }

        public IContainer Make(IRegistration registration)
        {
            var finder = _finderFactory.Make(registration);
            _wrapper.SetFinder(finder);
            return _containerFactory.Make(registration);
        }
    }
}
