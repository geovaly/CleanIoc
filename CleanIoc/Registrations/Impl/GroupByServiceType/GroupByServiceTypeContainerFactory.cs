using CleanIoc.Builder;

namespace CleanIoc.Registrations.Impl.GroupByServiceType
{
    class GroupByServiceTypeContainerFactory : IContainerFactory
    {
        private readonly IContainerFactory _factory;
        private readonly ServiceTypeGrouping _serviceTypeGrouping;

        public GroupByServiceTypeContainerFactory(IContainerFactory factory, ServiceTypeGrouping serviceTypeGrouping)
        {
            _factory = factory;
            _serviceTypeGrouping = serviceTypeGrouping;
        }

        public IContainer Make(IRegistration registration)
        {
            return _factory.Make(_serviceTypeGrouping.Group(registration));
        }
    }
}
