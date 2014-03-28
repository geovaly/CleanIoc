using FakeItEasy;
using FluentAssertions;
using CleanIoc.Core;
using NUnit.Framework;

namespace CleanIoc.Tests.UnitTests
{
    [TestFixture]
    class ContainerTests
    {
        [Test]
        public void Resolve_ShouldGetTheInstanceFrom_InstanceLookup()
        {
            InstanceLookup<IService> instanceLookup = scope => new ServiceImpl();

            var finder = A.Fake<IInstanceLookupFinder>();
            A.CallTo(() => finder.Find<IService>())
                .Returns(instanceLookup);

            var container = MakeContainer(instanceLookupFinder: finder);

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

        [Test]
        public void Resolve_ShouldGetTheInstanceFrom_InstanceLookup_v2()
        {
            InstanceLookup<IService> instanceLookup = scope => new ServiceImpl();

            var finder = A.Fake<IInstanceLookupFinder>();
            A.CallTo(() => finder.Find(typeof(IService)))
                .Returns(instanceLookup);

            var container = MakeContainer(instanceLookupFinder: finder);

            var instance = container.Resolve(typeof(IService));

            instance.Should().BeOfType<ServiceImpl>();
        }

        [Test]
        public void ContainerTag_ShouldBeTheRootTag()
        {
            var container = MakeContainer() as ILifetimeScope;

            container.Tag.Should().BeSameAs("root");
        }

        [Test]
        public void BeginScope_ShouldCreateAnAnonymousLifetimeScope()
        {
            var container = MakeContainer();

            ILifetimeScope scope = container.BeginScope();

            scope.Tag.Should().Be("anonymous");
        }

        [Test]
        public void BeginScopeWithTag_ShouldCreateATaggedLifetimeScope()
        {
            var container = MakeContainer();

            ILifetimeScope scope = container.BeginScope("tag");

            scope.Tag.Should().Be("tag");
        }

        [Test]
        public void BeginScopeWithTagAndWithState_ShouldCreateATaggedLifetimeScopeThatHaveAState()
        {
            var container = MakeContainer();

            ILifetimeScope scope = container.BeginScope("tag", "state");

            scope.Tag.Should().Be("tag");
            scope.State.Should().Be("state");
        }

        [Test]
        public void EveryScopeShouldHaveATagIndexAssociatedWithTheTag()
        {
            const int expectedTagIndex = 1;

            var tagDetailsFinder = A.Fake<ITagDetailsFinder>();

            A.CallTo(() => tagDetailsFinder.Find("tag"))
                .Returns(new TagDetails(expectedTagIndex));

            var scope = (LifetimeScope)MakeContainer(tagDetailsFinder: tagDetailsFinder).BeginScope("tag");

            scope.TagIndex.Should().Be(expectedTagIndex);
        }

        [Test]
        public void EveryScopeExceptTheContainerShouldHaveAParent()
        {
            var container = MakeContainer();
            var scope1 = (LifetimeScope)container.BeginScope();
            var scope2 = (LifetimeScope)scope1.BeginScope("tag");

            container.Parent.Should().BeNull();
            scope1.Parent.Should().Be(container);
            scope2.Parent.Should().Be(scope1);
        }

        [Test]
        public void EveryScopeShouldHaveSingletons()
        {
            const int singletonsCount = 1;

            var finder = A.Fake<ITagDetailsFinder>();
            A.CallTo(() => finder.Find("tag"))
                .Returns(new TagDetails(0, singletonsCount));

            var container = MakeContainer(tagDetailsFinder: finder);

            var scope = (LifetimeScope)container.BeginScope("tag");

            scope.Singletons.Length.Should().Be(singletonsCount);
        }

        [Test]
        public void EveryScopeShouldHaveRootSingletons()
        {
            const int singletonsCount = 1;

            var finder = A.Fake<ITagDetailsFinder>();
            A.CallTo(() => finder.Find("root"))
                .Returns(new TagDetails(0, singletonsCount));

            var container = MakeContainer(tagDetailsFinder: finder);
            var scope1 = (LifetimeScope)container.BeginScope();
            var scope2 = (LifetimeScope)scope1.BeginScope();

            container.Singletons.Length.Should().Be(singletonsCount);

            container.Singletons.Should().Equal(container.RootSingletons)
                .And.Equal(scope1.RootSingletons)
                .And.Equal(scope2.RootSingletons);
        }

        [Test]
        public void EveryScopeShouldHaveConstants()
        {
            var expectedConstants = new object[1];
            var container = MakeContainer(expectedConstants);
            var scope1 = (LifetimeScope)container.BeginScope();
            var scope2 = (LifetimeScope)scope1.BeginScope();

            expectedConstants.Should().Equal(container.Constants)
                .And.Equal(scope1.Constants)
                .And.Equal(scope2.Constants);
        }


        private Container MakeContainer(
            object[] constants = null,
            IInstanceLookupFinder instanceLookupFinder = null,
            ITagDetailsFinder tagDetailsFinder = null)
        {
            return new Container(
                constants ?? new object[0],
                instanceLookupFinder ?? A.Dummy<IInstanceLookupFinder>(),
                tagDetailsFinder ?? A.Dummy<ITagDetailsFinder>());
        }
    }
}
