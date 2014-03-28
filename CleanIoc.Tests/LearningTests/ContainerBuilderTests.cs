using FluentAssertions;
using CleanIoc.Builder;
using NUnit.Framework;

namespace CleanIoc.Tests.LearningTests
{
    [TestFixture]
    class ContainerBuilderTests
    {
        [Test]
        public void Resolve_RegisterTypeWithImplementation_ReturnsAnInstanceOfImplementationType()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<IService, ServiceImpl>()
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

        [Test]
        public void Resolve_RegisterTypeWithDelegate_ReturnsAnInstanceFromDelegate()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<IService>(scope => new ServiceImpl())
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

        [Test]
        public void Resolve_RegisterInstance_ReturnsThatInstance()
        {
            IService expectedInstance = new ServiceImpl();

            IContainer container = new ContainerBuilder()
                .RegisterInstance(expectedInstance)
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().Be(expectedInstance);
        }

        [Test]
        public void Resolve_RegisterConcreteType_ReturnsAnInstanceOfConcreteType()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>()
                .Build();

            var instance = container.Resolve<Class1>();

            instance.Should().NotBeNull();
        }

        [Test]
        public void Resolve_RegisterTypeWithTransientLifestyle_ReturnsAlwaysANewInstance()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>(Lifestyle.Transient)
                .Build();

            var instance = container.Resolve<Class1>();
            var anotherInstance = container.Resolve<Class1>();

            instance.Should().NotBeSameAs(anotherInstance);
        }

        [Test]
        public void RegisterType_WithNoLifestyle_ShouldBeTransient()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>()
                .Build();

            var instance = container.Resolve<Class1>();
            var anotherInstance = container.Resolve<Class1>();

            instance.Should().NotBeSameAs(anotherInstance);
        }

        [Test]
        public void Resolve_RegisterTypeWithSingletonLifestyle_ReturnsAlwaysTheSameInstance()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>(Lifestyle.Singleton)
                .Build();

            var instance = container.Resolve<Class1>();
            var anotherInstance = container.Resolve<Class1>();

            instance.Should().BeSameAs(anotherInstance);
        }

        [Test]
        public void Resolve_RegisterTypeWithSingletonPerScopeLifestyle_ReturnsTheSameInstancePerScope()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>(Lifestyle.SingletonPerScope())
                .Build();

            var scope1 = container.BeginScope();
            var scope2 = container.BeginScope();

            var instanceFromScope1 = scope1.Resolve<Class1>();
            var anotherInstanceFromScope1 = scope1.Resolve<Class1>();
            var instanceFromScope2 = scope2.Resolve<Class1>();

            instanceFromScope1.Should().BeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_RegisterType_WithTransientPerScopeLifestyle_ReturnsANewInstancePerScope()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Class1>(Lifestyle.TransientPerScope())
                .Build();

            var scope1 = container.BeginScope();
            var scope2 = container.BeginScope();

            var instanceFromScope1 = scope1.Resolve<Class1>();
            var anotherInstanceFromScope1 = scope1.Resolve<Class1>();
            var instanceFromScope2 = scope2.Resolve<Class1>();

            instanceFromScope1.Should().NotBeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_RegisterTypeWithDifferentImplementationsPerTag_ReturnsTheImplementationFromTheCurrentScope()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<IService, ServiceImpl>(Lifestyle.Transient)
                .RegisterType<IService, ServiceImpl2>(Lifestyle.TransientPerScope("tag1"))
                .RegisterType<IService, ServiceImpl3>(Lifestyle.SingletonPerScope("tag2"))
                .Build();

            var instanceFromContainer = container
                .Resolve<IService>();

            var instanceFromScopeOfTag1 = container.BeginScope("tag1")
                .Resolve<IService>();

            var instanceFromScopeOfTag2 = container.BeginScope("tag2")
                .Resolve<IService>();

            instanceFromContainer.Should().BeOfType<ServiceImpl>();
            instanceFromScopeOfTag1.Should().BeOfType<ServiceImpl2>();
            instanceFromScopeOfTag2.Should().BeOfType<ServiceImpl3>();
        }

        [Test]
        public void Resolve_RegisterStateFromScope_ReturnsTheStateFromTheScope()
        {
            var state = new ScopeState();

            IContainer container = new ContainerBuilder()
                .RegisterStateFromScope<ScopeState>("tag")
                .Build();

            var instance = container.BeginScope("tag", state).Resolve<ScopeState>();

            instance.Should().BeSameAs(state);
        }

        [Test]
        public void Resolve_LifetimeScope_ReturnsTheCurrentScope()
        {
            IContainer container = new ContainerBuilder().Build();
            var scope = container.BeginScope("tag");

            var scopeFromContainer = container.Resolve<ILifetimeScope>();
            var scopeFromScope = scope.Resolve<ILifetimeScope>();

            scopeFromContainer.Should().BeSameAs(container);
            scopeFromScope.Should().BeSameAs(scope);
        }

        [Test]
        public void ShouldDisposeTransientTypes_WhenCurrentScopeIsDisposed()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Disposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var currentScope = container.BeginScope())
            {
                instance = currentScope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldDisposeSingletonTypes_WhenContainerIsDisposed()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Disposable>(Lifestyle.Singleton)
                .Build();

            Disposable instance;
            using (var currentScope = container.BeginScope())
            {
                instance = currentScope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeFalse();

            container.Dispose();
            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldDisposeSingletonPerScopeTypes_WhenAssociatedScopeForTheInstanceIsDisposed()
        {
            IContainer container = new ContainerBuilder()
                .RegisterType<Disposable>(Lifestyle.SingletonPerScope("tag"))
                .Build();

            Disposable instance;
            using (var associatedScope = container.BeginScope("tag"))
            {
                using (var currentScope = associatedScope.BeginScope())
                {
                    instance = currentScope.Resolve<Disposable>();
                }

                instance.IsDisposeCalled.Should().BeFalse();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldDisposeTransientPerScopeTypes_WhenCurrentScopeIsDisposed()
        {
            IContainer container = new ContainerBuilder()
               .RegisterType<Disposable>(Lifestyle.TransientPerScope("tag"))
               .Build();

            Disposable instance;
            using (var associatedScope = container.BeginScope("tag"))
            {
                using (var currentScope = associatedScope.BeginScope())
                {
                    instance = currentScope.Resolve<Disposable>();
                }

                instance.IsDisposeCalled.Should().BeTrue();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void ShouldDisposeInstances_WhenContainerIsDisposed()
        {
            var instance = new Disposable();

            var container = new ContainerBuilder()
                .RegisterInstance(instance)
                .Build();

            container.Dispose();
            instance.IsDisposeCalled.Should().BeTrue();
        }
    }


}
