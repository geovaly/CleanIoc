using System;
using FluentAssertions;
using CleanIoc.Builder;
using NUnit.Framework;

namespace CleanIoc.Tests.AcceptanceTests
{
    [TestFixture]
    class BadConfigurationExceptionTests : BaseTests
    {

        [Test]
        public void Resolve_unregistered_type_should_throw_exception()
        {
            var container = AContainer().Build();

            ShouldThrow<BadConfigurationException>(() => 
                container.Resolve<ClassNotRegistered>());
        }

        [Test]
        public void Inject_unregistered_type_should_throw_exception()
        {
            var container = AContainer()
                .RegisterType<ClassWith_ClassNotRegistered>()
                .Build();

            ShouldThrow<BadConfigurationException>(() =>
                container.Resolve<ClassWith_ClassNotRegistered>());
        }

        [Test]
        public void Resolve_type_with_no_public_constructor_should_throw_exception()
        {
            var container = AContainer()
                .RegisterType<ClassWithNoPublicConstructor>()
                .Build();

            ShouldThrow<BadConfigurationException>(() =>
               container.Resolve<ClassWithNoPublicConstructor>());
        }

        [Test]
        public void Resolve_type_with_more_than_one_public_constructor_should_throw_exception()
        {
            var container = AContainer()
                .RegisterType<ClassWithManyPublicConstructors>()
                .Build();

            ShouldThrow<BadConfigurationException>(() =>
               container.Resolve<ClassWithManyPublicConstructors>());
        }

        private void ShouldThrow<TException>(Action action) where TException : Exception
        {
            action.ShouldThrow<TException>();
        }
    }
}
