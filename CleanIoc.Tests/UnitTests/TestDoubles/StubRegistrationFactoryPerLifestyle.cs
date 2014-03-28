using FakeItEasy;
using CleanIoc.Builder;
using CleanIoc.Builder.Lifestyles;
using CleanIoc.Registrations;

namespace CleanIoc.Tests.UnitTests.TestDoubles
{
    static class StubRegistrationFactoryPerLifestyle
    {
        public static IRegistrationFactoryPerLifestyle ReturnRegistrationForTransientLifestyle(IRegistration registration)
        {
            var regPerLifestyleFactory = A.Fake<IRegistrationFactoryPerLifestyle>();

            A.CallTo(() => regPerLifestyleFactory.Make(A<TransientLifestyle>._))
                .Returns(registration);

            return regPerLifestyleFactory;
        }

        public static IRegistrationFactoryPerLifestyle ReturnRegistrationForSingletonPerScopeLifestyle(
            IRegistration registration,
            Lifestyle lifestyle)
        {
            var regPerLifestyleFactory = A.Fake<IRegistrationFactoryPerLifestyle>();

            A.CallTo(() => regPerLifestyleFactory.Make(A<SingletonPerScopeLifestyle>.That.Matches(x => x.Equals(lifestyle))))
                .Returns(registration);

            return regPerLifestyleFactory;
        }

        public static IRegistrationFactoryPerLifestyle ReturnRegistrationForTransientPerScopeLifestyle(
           IRegistration registration,
           Lifestyle lifestyle)
        {
            var regPerLifestyleFactory = A.Fake<IRegistrationFactoryPerLifestyle>();

            A.CallTo(() => regPerLifestyleFactory.Make(A<TransientPerScopeLifestyle>.That.Matches(x => x.Equals(lifestyle))))
                .Returns(registration);

            return regPerLifestyleFactory;
        }

        public static IRegistrationFactoryPerLifestyle ReturnRegistrationForSingletonLifestyle(IRegistration registration)
        {
            var regPerLifestyleFactory = A.Fake<IRegistrationFactoryPerLifestyle>();

            A.CallTo(() => regPerLifestyleFactory.Make(A<SingletonLifestyle>._))
                .Returns(registration);

            return regPerLifestyleFactory;
        }
    }
}
