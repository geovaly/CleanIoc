using FluentAssertions;
using CleanIoc.Builder;
using NUnit.Framework;

namespace CleanIoc.Tests.AcceptanceTests
{
    [TestFixture]
    class HighPerformanceContainerTest : BaseTests
    {
        /// <summary>
        /// Because CreateHighPerformanceContainerBuilder can be called only 
        /// one time we can't use more than one test method.
        /// </summary>
        [Test]
        public void Resolve_service_type_with_implementation_type()
        {
            var container = AContainer()
                .RegisterType<IService, ServiceImpl>()
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

        protected override ContainerBuilder AContainer()
        {
            return ContainerBuilder.CreateHighPerformanceContainerBuilder();
        }
    }
}
