using CleanIoc.Builder.Lifestyles;
using CleanIoc.Registrations;

namespace CleanIoc.Builder
{
    interface IRegistrationFactoryPerLifestyle
    {
        IRegistration Make(SingletonLifestyle lifestyle);

        IRegistration Make(TransientLifestyle lifestyle);

        IRegistration Make(SingletonPerScopeLifestyle lifestyle);

        IRegistration Make(TransientPerScopeLifestyle lifestyle);
    }
}
