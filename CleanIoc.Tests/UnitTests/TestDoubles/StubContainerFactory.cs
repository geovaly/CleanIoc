using FakeItEasy;
using CleanIoc.Builder;
using CleanIoc.Registrations;

namespace CleanIoc.Tests.UnitTests.TestDoubles
{
    static class StubContainerFactory
    {
        public static IContainerFactory ReturnContainerForRegistration(IContainer containerResult, IRegistration registration)
        {
            var containerFactory = A.Fake<IContainerFactory>();

            A.CallTo(() => containerFactory.Make(A<IRegistration>.That.Matches(
                x => x.LeafRegistrations().Contains(registration)))).Returns(containerResult);

            return containerFactory;
        }
    }
}
