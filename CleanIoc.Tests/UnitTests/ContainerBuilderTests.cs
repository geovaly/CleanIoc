using System;
using CleanIoc.Tests.UnitTests.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using CleanIoc.Builder;
using NUnit.Framework;

namespace CleanIoc.Tests.UnitTests
{
    [TestFixture]
    class ContainerBuilderTests
    {
        [Test]
        public void CanBuildContainerOnlyOneTime()
        {
            var builder = new ContainerBuilder(A.Dummy<IContainerFactory>(), A.Dummy<IRegistrationFactory>());

            Action buildAction = () => builder.Build();

            buildAction.ShouldNotThrow();
            buildAction.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void RegisterATransientTypeWithImplementation()
        {
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForReflection<IService, ServiceImpl>(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForTransientLifestyle(newRegistration)));

            var container = builder.RegisterType<IService, ServiceImpl>(Lifestyle.Transient).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterASingletonTypeWithImplementation()
        {
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForReflection<IService, ServiceImpl>(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForSingletonLifestyle(newRegistration)));

            var container = builder.RegisterType<IService, ServiceImpl>(Lifestyle.Singleton).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterASingletonPerScopeTypeWithImplementation()
        {
            Lifestyle lifestyle = Lifestyle.SingletonPerScope("tag1", "tag2");

            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForReflection<IService, ServiceImpl>(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForSingletonPerScopeLifestyle(
                        newRegistration, lifestyle)));

            var container = builder.RegisterType<IService, ServiceImpl>(lifestyle).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterATransientPerScopeTypeWithImplementation()
        {
            Lifestyle lifestyle = Lifestyle.TransientPerScope("tag1", "tag2");
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForReflection<IService, ServiceImpl>(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForTransientPerScopeLifestyle(
                        newRegistration, lifestyle)));

            var container = builder.RegisterType<IService, ServiceImpl>(lifestyle).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterATransientTypeWithDelegate()
        {
            var expectedContainer = A.Fake<IContainer>();
            Func<ILifetimeScope, IService> instanceLookup = scope => new ServiceImpl();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForDelegate(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForTransientLifestyle(newRegistration),
                    instanceLookup));

            var container = builder.RegisterType(instanceLookup, Lifestyle.Transient).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterASingletonTypeWithDelegate()
        {
            var expectedContainer = A.Fake<IContainer>();
            Func<ILifetimeScope, IService> instanceLookup = scope => new ServiceImpl();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForDelegate(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForSingletonLifestyle(newRegistration),
                    instanceLookup));

            var container = builder.RegisterType(instanceLookup, Lifestyle.Singleton).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterInstance()
        {
            var expectedContainer = A.Fake<IContainer>();
            IService instance = new ServiceImpl();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnRegistrationForInstance(newRegistration, instance));

            var container = builder.RegisterInstance(instance).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterStateForScope()
        {
            var expectedContainer = A.Fake<IContainer>();
            var state = new ScopeState();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnRegistrationForState(newRegistration, state, new object[] { "tag" }));

            var container = builder.RegisterStateFromScope<ScopeState>("tag").Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void ConstructorShouldRegisterForLifetimeScope()
        {
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var container = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnRegistrationForLifetimeScope(newRegistration))
                .Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterASingletonPerScopeTypeWithDelegate()
        {
            Lifestyle lifestyle = Lifestyle.SingletonPerScope("tag1", "tag2");

            Func<ILifetimeScope, IService> instanceLookup = scope => new ServiceImpl();
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForDelegate(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForSingletonPerScopeLifestyle(newRegistration, lifestyle),
                    instanceLookup));

            var container = builder.RegisterType(instanceLookup, lifestyle).Build();

            container.Should().BeSameAs(expectedContainer);
        }

        [Test]
        public void RegisterATransientPerScopeTypeWithDelegate()
        {
            Lifestyle lifestyle = Lifestyle.TransientPerScope("tag1", "tag2");

            Func<ILifetimeScope, IService> instanceLookup = scope => new ServiceImpl();
            var expectedContainer = A.Fake<IContainer>();
            var newRegistration = new FakeRegistration<IService>();

            var builder = new ContainerBuilder(
                StubContainerFactory.ReturnContainerForRegistration(expectedContainer, newRegistration),
                StubRegistrationFactory.ReturnFactoryForDelegate(
                    StubRegistrationFactoryPerLifestyle.ReturnRegistrationForTransientPerScopeLifestyle(
                        newRegistration, lifestyle),
                    instanceLookup));

            var container = builder.RegisterType(instanceLookup, lifestyle).Build();

            container.Should().BeSameAs(expectedContainer);
        }


    }
}

