using System;
using System.Reflection;
using CleanIoc.Utility;

namespace CleanIoc.Builder
{
    public static class ContainerBuilderExtensions
    {
        private static readonly MethodInfo RegisterTypeWithImplementationMethod
            = ReflectionHelpers.GetMethod<ContainerBuilder>(x => x.RegisterType<object, object>(null))
                           .GetGenericMethodDefinition();

        public static ContainerBuilder RegisterType<TService>(this ContainerBuilder builder)
            where TService : class
        {
            builder.RegisterType<TService>(Lifestyle.Transient);
            return builder;
        }

        public static ContainerBuilder RegisterType<TService>(this ContainerBuilder builder, Lifestyle lifestyle)
            where TService : class
        {
            builder.RegisterType<TService, TService>(lifestyle);
            return builder;
        }

        public static ContainerBuilder RegisterType<TService, TImpl>(this ContainerBuilder builder)
            where TService : class
            where TImpl : class, TService
        {
            builder.RegisterType<TService, TImpl>(Lifestyle.Transient);
            return builder;
        }

        public static ContainerBuilder RegisterType<TService>(this ContainerBuilder builder, Func<ILifetimeScope, TService> instanceLookup)
            where TService : class
        {
            builder.RegisterType(instanceLookup, Lifestyle.Transient);
            return builder;
        }

        public static ContainerBuilder RegisterType(this ContainerBuilder builder, Type serviceType, Type concreteType, Lifestyle lifestyle)
        {
            RegisterTypeWithImplementationMethod.MakeGenericMethod(serviceType, concreteType).Invoke(builder, new object[] { lifestyle });
            return builder;
        }

        public static ContainerBuilder RegisterType(this ContainerBuilder builder, Type serviceType, Type concreteType)
        {
            return RegisterType(builder, serviceType, concreteType, Lifestyle.Transient);
        }

        public static ContainerBuilder RegisterType(this ContainerBuilder builder, Type concreteType, Lifestyle lifestyle)
        {
            return RegisterType(builder, concreteType, concreteType, lifestyle);
        }

        public static ContainerBuilder RegisterType(this ContainerBuilder builder, Type concreteType)
        {
            return RegisterType(builder, concreteType, Lifestyle.Transient);
        }

    }
}
