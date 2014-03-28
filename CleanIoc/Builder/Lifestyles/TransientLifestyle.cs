using CleanIoc.Registrations;

namespace CleanIoc.Builder.Lifestyles
{
    class TransientLifestyle : Lifestyle
    {
        internal override IRegistration MakeRegistration(IRegistrationFactoryPerLifestyle factory)
        {
            return factory.Make(this);
        }
    }
}
