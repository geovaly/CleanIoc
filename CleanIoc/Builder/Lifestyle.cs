using System.Linq;
using CleanIoc.Builder.Lifestyles;
using CleanIoc.Core;
using CleanIoc.Registrations;

namespace CleanIoc.Builder
{
    public abstract class Lifestyle
    {
        public static readonly Lifestyle Transient = new TransientLifestyle();
        public static readonly Lifestyle Singleton = new SingletonLifestyle();

        public static Lifestyle SingletonPerScope(params object[] tags)
        {
            return new SingletonPerScopeLifestyle(NotEmpty(tags));
        }

        public static Lifestyle TransientPerScope(params object[] tags)
        {
            return new TransientPerScopeLifestyle(NotEmpty(tags));
        }

        private static object[] NotEmpty(object[] tags)
        {
            return tags.Any()
                ? tags
                : new[] { LifetimeScope.AnonymousTag };
        }


        internal abstract IRegistration MakeRegistration(IRegistrationFactoryPerLifestyle factory);

    }
}
