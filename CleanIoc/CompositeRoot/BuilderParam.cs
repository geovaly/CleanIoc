using CleanIoc.Builder;

namespace CleanIoc.CompositeRoot
{
    struct BuilderParam
    {
        public readonly IRegistrationFactory RegistrationFactory;
        public readonly IContainerFactory ContainerFactory;

        public BuilderParam(IRegistrationFactory registrationFactory, IContainerFactory containerFactory)
        {
            RegistrationFactory = registrationFactory;
            ContainerFactory = containerFactory;
        }
    }
}
