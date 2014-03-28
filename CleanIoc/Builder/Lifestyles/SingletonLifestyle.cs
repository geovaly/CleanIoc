using CleanIoc.Registrations;

namespace CleanIoc.Builder.Lifestyles
{
    class SingletonLifestyle : Lifestyle
    {
        internal override IRegistration MakeRegistration(IRegistrationFactoryPerLifestyle factory)
        {
            return factory.Make(this);
        }
    }
}
