using CleanIoc.Registrations;

namespace CleanIoc.Builder.Lifestyles
{
    class TransientPerScopeLifestyle : Lifestyle
    {
        public readonly object[] Tags;

        public TransientPerScopeLifestyle(object[] tags)
        {
            Tags = tags;
        }

        internal override IRegistration MakeRegistration(IRegistrationFactoryPerLifestyle factory)
        {
            return factory.Make(this);
        }
    }
}
