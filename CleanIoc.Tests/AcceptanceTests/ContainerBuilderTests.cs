using FluentAssertions;
using CleanIoc.Builder;
using CleanIoc.CompositeRoot;
using NUnit.Framework;

namespace CleanIoc.Tests.AcceptanceTests
{

    [TestFixture]
    internal class ContainerBuilderUsingDynamicAssembyTests : ContainerBuilderTests
    {
        protected override ContainerBuilder AContainer()
        {
            return new ContainerBuilder(new DynamicAssemblyBuilderParamProvider().Get());
        }
    }


    [TestFixture]
    class ContainerBuilderTests : BaseTests
    {

        [Test]
        public void Resolve_concrete_classes_that_have_a_parameterless_constructor()
        {
            var container = AContainer()
                .RegisterType<object>()
                .Build();

            var instance = container.Resolve<object>();

            instance.Should().NotBeNull();
        }

        [Test]
        public void Resolve_concrete_classes_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>()
                .RegisterType<ClassWithClass1>()
                .Build();

            var instance = container.Resolve<ClassWithClass1>();

            instance.Class1.Should().NotBeNull();
        }

        [Test]
        public void Inject_concrete_classes_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>()
                .RegisterType<ClassWithClass1>()
                .RegisterType<ClassWith_ClassWithClass1>()
                .Build();

            var instance = container.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;

            instance.Class1.Should().NotBeNull();
        }

        [Test]
        public void Resolve_Transient_that_have_a_parameterless_constructor()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.Transient)
                .Build();

            var instance1 = container.Resolve<object>();
            var instance2 = container.Resolve<object>();

            instance1.Should().NotBeSameAs(instance2);
        }

        [Test]
        public void Resolve_Transient_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Transient)
                .RegisterType<ClassWithClass1>(Lifestyle.Transient)
                .Build();

            var instance1 = container.Resolve<ClassWithClass1>();
            var instance2 = container.Resolve<ClassWithClass1>();

            instance1.Should().NotBeSameAs(instance2);
        }

        [Test]
        public void Inject_Transient_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Transient)
                .RegisterType<ClassWithClass1>(Lifestyle.Transient)
                .RegisterType<ClassWith_ClassWithClass1>(Lifestyle.Transient)
                .Build();

            var instance1 = container.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var instance2 = container.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;

            instance1.Should().NotBeSameAs(instance2);
        }

        [Test]
        public void Resolve_Singleton_that_have_a_parameterless_constructor()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.Singleton)
                .Build();

            var instance1 = container.Resolve<object>();
            var instance2 = container.Resolve<object>();

            instance1.Should().BeSameAs(instance2);
        }

        [Test]
        public void Resolve_Singleton_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Singleton)
                .RegisterType<ClassWithClass1>(Lifestyle.Singleton)
                .Build();

            var instance1 = container.Resolve<ClassWithClass1>();
            var instance2 = container.Resolve<ClassWithClass1>();

            instance1.Should().BeSameAs(instance2);
        }

        [Test]
        public void Inject_Singleton_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Singleton)
                .RegisterType<ClassWithClass1>(Lifestyle.Singleton)
                .RegisterType<ClassWith_ClassWithClass1>(Lifestyle.Transient)
                .Build();

            var instance1 = container.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var instance2 = container.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;

            instance1.Should().BeSameAs(instance2);
        }

        [Test]
        public void Resolve_SingletonPerScope_that_have_a_parameterless_constructor()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.SingletonPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<object>();
            var anotherInstanceFromScope1 = scope1.Resolve<object>();
            var instanceFromScope2 = scope2.Resolve<object>();

            instanceFromScope1.Should().BeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_SingletonPerScope_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Singleton)
                .RegisterType<ClassWithClass1>(Lifestyle.SingletonPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<ClassWithClass1>();
            var anotherInstanceFromScope1 = scope1.Resolve<ClassWithClass1>();
            var instanceFromScope2 = scope2.Resolve<ClassWithClass1>();

            instanceFromScope1.Should().BeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Inject_SingletonPerScope_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Singleton)
                .RegisterType<ClassWithClass1>(Lifestyle.SingletonPerScope("tag"))
                .RegisterType<ClassWith_ClassWithClass1>(Lifestyle.Transient)
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var anotherInstanceFromScope1 = scope1.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var instanceFromScope2 = scope2.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;

            instanceFromScope1.Should().BeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_SingletonPerScope_from_a_parent_scope()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.SingletonPerScope("tag"))
                .Build();

            var scope = container.BeginScope("tag").BeginScope("dummyTag");

            var instance = scope.Resolve<Class1>();

            instance.Should().NotBeNull();
        }

        [Test]
        public void Resolve_TransientPerScope_that_have_a_parameterless_constructor()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.TransientPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<object>();
            var anotherInstanceFromScope1 = scope1.Resolve<object>();
            var instanceFromScope2 = scope2.Resolve<object>();

            instanceFromScope1.Should().NotBeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_TransientPerScope_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Transient)
                .RegisterType<ClassWithClass1>(Lifestyle.TransientPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<ClassWithClass1>();
            var anotherInstanceFromScope1 = scope1.Resolve<ClassWithClass1>();
            var instanceFromScope2 = scope2.Resolve<ClassWithClass1>();

            instanceFromScope1.Should().NotBeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Inject_TransientPerScope_that_have_a_constructor_with_parameters()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.Transient)
                .RegisterType<ClassWithClass1>(Lifestyle.TransientPerScope("tag"))
                .RegisterType<ClassWith_ClassWithClass1>(Lifestyle.Transient)
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var anotherInstanceFromScope1 = scope1.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;
            var instanceFromScope2 = scope2.Resolve<ClassWith_ClassWithClass1>().ClassWithClass1;

            instanceFromScope1.Should().NotBeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_TransientPerScope_from_a_parent_scope()
        {
            var container = AContainer()
                .RegisterType<Class1>(Lifestyle.TransientPerScope("tag"))
                .Build();

            var scope = container.BeginScope("tag").BeginScope("dummyTag");

            var instance = scope.Resolve<Class1>();

            instance.Should().NotBeNull();
        }

        [Test]
        public void Resolve_RegisterType_WithImplementation()
        {
            var container = AContainer()
                .RegisterType<IService, ServiceImpl>()
                .Build();

            var instance = container.Resolve<IService>();

            instance.Should().BeOfType<ServiceImpl>();
        }

        [Test]
        public void Resolve_RegisterType_WithDelegate()
        {
            var expectedInstance = new object();

            var container = AContainer()
                .RegisterType(scope => expectedInstance)
                .Build();

            var instance = container.Resolve<object>();

            instance.Should().BeSameAs(expectedInstance);
        }


        [Test]
        public void Resolve_RegisterTransient_WithDelegate()
        {
            var container = AContainer()
                .RegisterType(scope => new object(), Lifestyle.Transient)
                .Build();

            var instance1 = container.Resolve<object>();
            var instance2 = container.Resolve<object>();

            instance1.Should().NotBeSameAs(instance2);
        }

        [Test]
        public void Resolve_RegisterSingleton_WithDelegate()
        {
            var container = AContainer()
                .RegisterType(scope => new object(), Lifestyle.Singleton)
                .Build();

            var instance1 = container.Resolve<object>();
            var instance2 = container.Resolve<object>();

            instance1.Should().BeSameAs(instance2);
        }

        [Test]
        public void Resolve_RegisterSingletonPerScope_WithDelegate()
        {
            var container = AContainer()
                .RegisterType(scope => new object(), Lifestyle.SingletonPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<object>();
            var anotherInstanceFromScope1 = scope1.Resolve<object>();
            var instanceFromScope2 = scope2.Resolve<object>();

            instanceFromScope1.Should().BeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_RegisterTransientPerScope_WithDelegate()
        {
            var container = AContainer()
                .RegisterType(scope => new object(), Lifestyle.TransientPerScope("tag"))
                .Build();

            var scope1 = container.BeginScope("tag");
            var scope2 = container.BeginScope("tag");

            var instanceFromScope1 = scope1.Resolve<object>();
            var anotherInstanceFromScope1 = scope1.Resolve<object>();
            var instanceFromScope2 = scope2.Resolve<object>();

            instanceFromScope1.Should().NotBeSameAs(anotherInstanceFromScope1)
                .And.NotBeSameAs(instanceFromScope2);
        }

        [Test]
        public void Resolve_service_with_different_implementations_per_tag()
        {
            var container = AContainer()
                .RegisterType<IService, ServiceImpl>(Lifestyle.Transient)
                .RegisterType<IService, ServiceImpl2>(Lifestyle.TransientPerScope("tag1"))
                .RegisterType<IService, ServiceImpl3>(Lifestyle.SingletonPerScope("tag2"))
                .Build();

            var instanceFromContainer = container.Resolve<IService>();

            var instanceFromScopeOfTag1 = container.BeginScope("tag1")
                .Resolve<IService>();

            var instanceFromScopeOfTag2 = container.BeginScope("tag2")
                .BeginScope("dummyTag").Resolve<IService>();

            instanceFromContainer.Should().BeOfType<ServiceImpl>();
            instanceFromScopeOfTag1.Should().BeOfType<ServiceImpl2>();
            instanceFromScopeOfTag2.Should().BeOfType<ServiceImpl3>();
        }

        [Test]
        public void Inject_service_with_different_implementations_per_tag()
        {
            var container = AContainer()
                .RegisterType<IService, ServiceImpl>(Lifestyle.Transient)
                .RegisterType<IService, ServiceImpl2>(Lifestyle.TransientPerScope("tag1"))
                .RegisterType<IService, ServiceImpl3>(Lifestyle.SingletonPerScope("tag2"))
                .RegisterType<ClassWithService>(Lifestyle.Transient)
                .Build();

            var instanceFromContainer = container.Resolve<ClassWithService>().Service;

            var instanceFromScopeOfTag1 = container.BeginScope("tag1")
                .Resolve<ClassWithService>().Service;

            var instanceFromScopeOfTag2 = container.BeginScope("tag2")
                .BeginScope("dummyTag").Resolve<ClassWithService>().Service;

            instanceFromContainer.Should().BeOfType<ServiceImpl>();
            instanceFromScopeOfTag1.Should().BeOfType<ServiceImpl2>();
            instanceFromScopeOfTag2.Should().BeOfType<ServiceImpl3>();
        }

        [Test]
        public void Inject_singleton_service_with_different_implementations_per_tag()
        {
            var container = AContainer()
                .RegisterType<IService, ServiceImpl2>(Lifestyle.SingletonPerScope("tag1"))
                .RegisterType<IService, ServiceImpl3>(Lifestyle.SingletonPerScope("tag2"))
                .RegisterType<ClassWithService>(Lifestyle.Transient)
                .Build();

            var instanceFromScopeOfTag1 = container.BeginScope("tag1")
                .Resolve<ClassWithService>().Service;

            var instanceFromScopeOfTag2 = container.BeginScope("tag2")
                .BeginScope("dummyTag").Resolve<ClassWithService>().Service;

            instanceFromScopeOfTag1.Should().BeOfType<ServiceImpl2>();
            instanceFromScopeOfTag2.Should().BeOfType<ServiceImpl3>();
        }

        [Test]
        public void Register_SingletonPerScope_with_many_tags()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.SingletonPerScope("tag1", "tag2"))
                .Build();

            var instanceFromTag1 = container.BeginScope("tag1").Resolve<object>();
            var instanceFromTag2 = container.BeginScope("tag2").Resolve<object>();

            instanceFromTag1.Should().NotBeNull();
            instanceFromTag2.Should().NotBeNull();
        }

        [Test]
        public void Register_SingletonPerAnonymousScope()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.SingletonPerScope())
                .Build();

            var instanceFromTag1 = container.BeginScope().Resolve<object>();

            instanceFromTag1.Should().NotBeNull();
        }

        [Test]
        public void Register_TransientPerAnonymousScope()
        {
            var container = AContainer()
                .RegisterType<object>(Lifestyle.TransientPerScope())
                .Build();

            var instanceFromTag1 = container.BeginScope().Resolve<object>();

            instanceFromTag1.Should().NotBeNull();
        }

        [Test]
        public void Resolve_LifetimeScope_ReturnsTheCurrentScope()
        {
            IContainer container = AContainer().Build();
            var scope = container.BeginScope();

            var scopeFromContainer = container.Resolve<ILifetimeScope>();
            var scopeFromScope = scope.Resolve<ILifetimeScope>();

            scopeFromContainer.Should().BeSameAs(container);
            scopeFromScope.Should().BeSameAs(scope);
        }

        [Test]
        public void Inject_LifetimeScope_WithCurrentScope()
        {
            var container = AContainer()
                .RegisterType<ClassWithLifetimeScope>()
                .Build();

            var scope = container.Resolve<ClassWithLifetimeScope>().LifetimeScope;

            scope.Should().BeSameAs(container);
        }

        [Test]
        public void Resolve_State_from_Scope()
        {
            var container = AContainer()
                .RegisterStateFromScope<string>("tag")
                .Build();

            var state = container.BeginScope("tag", "state").Resolve<string>();

            state.Should().Be("state");
        }

        [Test]
        public void Inject_State_from_Scope()
        {
            var container = AContainer()
                .RegisterStateFromScope<string>("tag")
                .RegisterType<ClassWithString>()
                .Build();

            var instance = container.BeginScope("tag", "state").Resolve<ClassWithString>();

            instance.String.Should().Be("state");
        }

        [Test]
        public void Should_dispose_SingletonPerScope_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.SingletonPerScope())
                .Build();

            Disposable instance;
            using (var currentScope = container.BeginScope())
            {
                instance = currentScope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_SingletonPerScope_instances_on_dispose_scope2()
        {
            var container = AContainer()
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
        public void Should_dispose_SingletonPerScope_instances_on_dispose_scope3()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.SingletonPerScope("tag"))
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var associatedScope = container.BeginScope("tag"))
            {
                using (var currentScope = associatedScope.BeginScope())
                {
                    instance = currentScope.Resolve<ClassWithDisposable>().Disposable;
                }

                instance.IsDisposeCalled.Should().BeFalse();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Singleton_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.Singleton)
                .Build();

            var instance = container.Resolve<Disposable>();
            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Singleton_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.Singleton)
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            var instance = container.Resolve<ClassWithDisposable>().Disposable;

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_TransientPerScope_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.TransientPerScope())
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_TransientPerScope_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.TransientPerScope("tag"))
                .Build();


            using (var associatedScope = container.BeginScope("tag"))
            {
                Disposable instance;
                using (var currentScope = associatedScope.BeginScope())
                {
                    instance = currentScope.Resolve<Disposable>();
                }
                instance.IsDisposeCalled.Should().BeTrue();
            }
        }


        [Test]
        public void Should_dispose_Transient_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.Transient)
                .Build();

            var instance = container.Resolve<Disposable>();
            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Transient_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Transient_instances_on_dispose_scope3()
        {
            var container = AContainer()
                .RegisterType<Disposable>(Lifestyle.Transient)
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<ClassWithDisposable>().Disposable;
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_SingletonPerScope_delegate_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.SingletonPerScope())
                .Build();

            Disposable instance;
            using (var scope1 = container.BeginScope())
            {
                instance = scope1.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }


        [Test]
        public void Should_dispose_SingletonPerScope_delegate_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.SingletonPerScope("tag"))
                .Build();

            Disposable instance;
            using (var scope1 = container.BeginScope("tag"))
            {
                using (var scope2 = scope1.BeginScope())
                {
                    instance = scope2.Resolve<Disposable>();
                }

                instance.IsDisposeCalled.Should().BeFalse();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_SingletonPerScope_delegate_instances_on_dispose_scope3()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.SingletonPerScope())
                 .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;

            using (var scope1 = container.BeginScope())
            {
                instance = scope1.Resolve<ClassWithDisposable>().Disposable;
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Singleton_delegate_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.Singleton)
                .Build();

            var instance = container.Resolve<Disposable>();

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Singleton_delegate_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.Singleton)
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            var instance = container.Resolve<ClassWithDisposable>().Disposable;

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_TransientPerScope_delegate_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.TransientPerScope())
                .Build();

            var scope1 = container.BeginScope();

            var instance = scope1.Resolve<Disposable>();

            scope1.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_TransientPerScope_delegate_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType(scope1 => new Disposable(), Lifestyle.TransientPerScope("tag"))
                .Build();

            using (var associatedScope = container.BeginScope("tag"))
            {
                Disposable instance;
                using (var currentScope = associatedScope.BeginScope())
                {
                    instance = currentScope.Resolve<Disposable>();
                }
                instance.IsDisposeCalled.Should().BeTrue();
            }
        }

        [Test]
        public void Should_dispose_TransientPerScope_delegate_instances_on_dispose_scope3()
        {
            var container = AContainer()
                .RegisterType(scope1 => new Disposable(), Lifestyle.TransientPerScope())
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<ClassWithDisposable>().Disposable;
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Transient_delegate_instances_on_dispose_scope()
        {
            var container = AContainer()
                .RegisterType(scope => new Disposable(), Lifestyle.Transient)
                .Build();

            var instance = container.Resolve<Disposable>();

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Transient_delegate_instances_on_dispose_scope2()
        {
            var container = AContainer()
                .RegisterType(scope1 => new Disposable(), Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<Disposable>();
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Transient_delegate_instances_on_dispose_scope3()
        {
            var container = AContainer()
                .RegisterType(scope1 => new Disposable(), Lifestyle.Transient)
                .RegisterType<ClassWithDisposable>(Lifestyle.Transient)
                .Build();

            Disposable instance;
            using (var scope = container.BeginScope())
            {
                instance = scope.Resolve<ClassWithDisposable>().Disposable;
            }

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Instance()
        {
            var container = AContainer()
                .RegisterInstance(new Disposable())
                .Build();

            var instance = container.Resolve<Disposable>();

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }

        [Test]
        public void Should_dispose_Instance_v2()
        {
            var container = AContainer()
                .RegisterInstance(new Disposable())
                .RegisterType<ClassWithDisposable>()
                .Build();

            var instance = container.Resolve<ClassWithDisposable>().Disposable;

            container.Dispose();

            instance.IsDisposeCalled.Should().BeTrue();
        }
    }
}
