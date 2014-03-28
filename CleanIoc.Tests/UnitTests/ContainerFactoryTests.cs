using CleanIoc.Tests.UnitTests.TestDoubles;
using FluentAssertions;
using CleanIoc.Core;
using CleanIoc.Factory;
using CleanIoc.Factory.Impl;
using CleanIoc.Factory.Impl.Constants;
using CleanIoc.Factory.Impl.InstanceLookups;
using NUnit.Framework;

namespace CleanIoc.Tests.UnitTests
{
    [TestFixture]
    class ContainerFactoryTests
    {
        [Test]
        public void ShouldInitializeTags()
        {
            int tag1Index = -1;
            int tag2Index = -1;

            var registration = new FakeRegistration<object>
            {
                OnInitTagsCallback = initializer =>
                {
                    initializer.AddTagIfNotExists("tag1");
                    initializer.AddTagIfNotExists("tag2");
                    tag1Index = initializer.GetIndexOfAddedTag("tag1");
                    tag2Index = initializer.GetIndexOfAddedTag("tag2");
                }
            };

            var container = MakeContainerFactory().Make(registration);

            var scopeOfTag1 = (LifetimeScope)container.BeginScope("tag1");
            var scopeOfTag2 = (LifetimeScope)container.BeginScope("tag2");

            scopeOfTag1.TagIndex.Should().Be(tag1Index);
            scopeOfTag2.TagIndex.Should().Be(tag2Index);
        }

        [Test]
        public void ShouldInitializeSingletons()
        {
            int singletonIndex0OfTag1 = -1, singletonIndex1OfTag1 = -1;
            int singletonIndex0OfTag2 = -1;

            var registration = new FakeRegistration<object>
            {
                OnInitSingletonsCallback = initializer =>
                {
                    initializer.AddSingletonFor("tag1");
                    singletonIndex0OfTag1 = initializer.GetIndexOfLastAddedSingletonFor("tag1");
                    initializer.AddSingletonFor("tag1");
                    singletonIndex1OfTag1 = initializer.GetIndexOfLastAddedSingletonFor("tag1");
                    initializer.AddSingletonFor("tag2");
                    singletonIndex0OfTag2 = initializer.GetIndexOfLastAddedSingletonFor("tag2");
                }
            };

            var container = MakeContainerFactory().Make(registration);

            var scopeOfTag1 = (LifetimeScope)container.BeginScope("tag1");
            var scopeOfTag2 = (LifetimeScope)container.BeginScope("tag2");

            scopeOfTag1.Singletons.Length.Should().Be(2);
            scopeOfTag2.Singletons.Length.Should().Be(1);
            singletonIndex0OfTag1.Should().Be(0);
            singletonIndex1OfTag1.Should().Be(1);
            singletonIndex0OfTag2.Should().Be(0);
        }

        [Test]
        public void ShouldInitializeInstanceLookups()
        {
            var expectedInstance = new ServiceImpl();
            var registration = new FakeRegistration<IService>
            {
                MakeInstanceLookupResult = scope => expectedInstance
            };
            var container = MakeContainerFactory().Make(registration);

            var instance = container.Resolve<IService>();

            instance.Should().BeSameAs(expectedInstance);
        }


        [Test]
        public void ShouldInitializeConstants()
        {
            const string constant1 = "Constant1";
            const string constant2 = "Constant2";

            var registration = new FakeRegistration<IService>
            {
                OnMakingConstantsCallback = factory =>
                {
                    factory.MakeConstant(constant1);
                    factory.MakeConstant(constant2);
                }
            };

            var container = (LifetimeScope)MakeContainerFactory().Make(registration);

            container.Constants.Should().Equal(new object[] { constant1, constant2 });

        }

        private ContainerFactory MakeContainerFactory()
        {
            return new ContainerFactory(
                () => new ContainerConstantsBuilder(),
                () => new DictionaryInstanceLookupsBuilder(),
                () => new DictionaryTagDetailsBuilder());
        }
    }
}
