using System;

namespace CleanIoc.Tests
{
    public sealed class Class1 { }

    public sealed class Class2 { }

    public sealed class ClassWithClass1
    {
        public readonly Class1 Class1;

        public ClassWithClass1(Class1 class1)
        {
            Class1 = class1;
        }
    }

    public sealed class ClassWith_ClassWithClass1
    {
        public readonly ClassWithClass1 ClassWithClass1;

        public ClassWith_ClassWithClass1(ClassWithClass1 classWithClass1)
        {
            ClassWithClass1 = classWithClass1;
        }
    }

    public interface IService
    {

    }

    public sealed class ServiceImpl : IService
    {

    }

    public sealed class ServiceImpl2 : IService
    {

    }

    public sealed class ServiceImpl3 : IService
    {

    }

    public sealed class ClassWithService
    {
        public readonly IService Service;

        public ClassWithService(IService service)
        {
            Service = service;
        }
    }

    public sealed class ClassWithLifetimeScope
    {
        public readonly ILifetimeScope LifetimeScope;

        public ClassWithLifetimeScope(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }
    }

    public sealed class ClassWithString
    {
        public readonly String String;

        public ClassWithString(string s)
        {
            String = s;
        }
    }

    public class Disposable : IDisposable
    {
        public bool IsDisposeCalled { get; private set; }

        public void Dispose()
        {
            if (IsDisposeCalled)
                throw new InvalidOperationException();

            IsDisposeCalled = true;
        }
    }

    public class ClassWithDisposable
    {
        public readonly Disposable Disposable;

        public ClassWithDisposable(Disposable disposable)
        {
            Disposable = disposable;
        }
    }

    public class ClassWithManyPublicConstructors
    {
        public ClassWithManyPublicConstructors(object obj)
        {
        }

        public ClassWithManyPublicConstructors()
        {
        }

    }

    public class ClassWithNoPublicConstructor
    {
        private ClassWithNoPublicConstructor()
        {
        }

    }

    public sealed class ClassNotRegistered { }


    public sealed class ClassWith_ClassNotRegistered
    {
        public readonly ClassNotRegistered ClassNotRegistered;

        public ClassWith_ClassNotRegistered(ClassNotRegistered classNotRegistered)
        {
            ClassNotRegistered = classNotRegistered;
        }
    }

    class ScopeState
    {
    }
}
