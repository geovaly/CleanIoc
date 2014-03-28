using FluentAssertions;
using NUnit.Framework;
using CleanIoc.Builder;

namespace CleanIoc.Tests.AcceptanceTests
{
    [TestFixture]
    class ContainerBuilderExtensionsTests : BaseTests
    {
        [Test]
        public void Register_concrete_type()
        {
            var container = AContainer()
                .RegisterType(typeof(Class1))
                .Build();

            var instance = container.Resolve<Class1>();

            instance.Should().BeOfType<Class1>();
        }

        [Test]
        public void Register_service_type_with_implementation()
        {
            var container = AContainer()
                .RegisterType(typeof(IService), typeof(ServiceImpl))
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

    }
}
