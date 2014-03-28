using CleanIoc.Registrations;

namespace CleanIoc.Builder.Lifestyles
{
    class SingletonPerScopeLifestyle : Lifestyle
    {
        public readonly object[] Tags;

        public SingletonPerScopeLifestyle(object[] tags)
        {
            Tags = tags;
        }
        internal override IRegistration MakeRegistration(IRegistrationFactoryPerLifestyle factory)
        {
            return factory.Make(this);
        }
    }
}
