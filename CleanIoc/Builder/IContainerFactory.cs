using CleanIoc.Registrations;

namespace CleanIoc.Builder
{
    interface IContainerFactory
    {
        IContainer Make(IRegistration registration);
    }
}
