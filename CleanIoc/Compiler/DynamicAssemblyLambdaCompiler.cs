using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Compiler
{
    class DynamicAssemblyLambdaCompiler : ILambdaCompiler
    {
        public const string DynamicAssemblyName = "CleanIoc.DynamicAssembly";

        private const string MethodName = "GetService";
        private static readonly Lazy<ModuleBuilder> LazyModuleBuilder = new Lazy<ModuleBuilder>(DefineDynamicModuleBuilder);
        private static int _typeId;

        public InstanceLookup<TService> Compile<TService>(Expression<InstanceLookup<TService>> expression)
            where TService : class
        {
            return CreateDelegateFromMethod<TService>(MethodName, MakeTypeInDynamicAssembly(expression));
        }

        private static Type MakeTypeInDynamicAssembly<TService>(Expression<InstanceLookup<TService>> expression)
            where TService : class
        {
            var typeBuilder = TypeBuilder();
            expression.CompileToMethod(MethodBuilder(typeof(TService), typeBuilder));
            return typeBuilder.CreateType();
        }

        private static TypeBuilder TypeBuilder()
        {
            return LazyModuleBuilder.Value.DefineType(TypeName(), TypeAttributes());
        }

        private static string TypeName()
        {
            return "Factory" + Interlocked.Increment(ref _typeId);
        }

        private static TypeAttributes TypeAttributes()
        {
            return System.Reflection.TypeAttributes.Public |
                   System.Reflection.TypeAttributes.Sealed |
                   System.Reflection.TypeAttributes.Abstract;
        }

        private static MethodBuilder MethodBuilder(Type serviceType, TypeBuilder typeBuilder)
        {
            return typeBuilder.DefineMethod(
                MethodName,
                MethodAttributes(),
                serviceType,
                new[] { typeof(LifetimeScope) });
        }

        private static MethodAttributes MethodAttributes()
        {
            return System.Reflection.MethodAttributes.Public |
                   System.Reflection.MethodAttributes.Static;
        }

        private static InstanceLookup<TService> CreateDelegateFromMethod<TService>(string methodName, Type type)
            where TService : class
        {
            return (InstanceLookup<TService>)Delegate.CreateDelegate(
                typeof(InstanceLookup<TService>),
                type.GetMethod(methodName));
        }

        private static ModuleBuilder DefineDynamicModuleBuilder()
        {
            var assemblyName = new AssemblyName(DynamicAssemblyName);
            return AssemblyBuilder(assemblyName).DefineDynamicModule(assemblyName.Name);
        }

        private static AssemblyBuilder AssemblyBuilder(AssemblyName assemblyName)
        {
            return AppDomain.CurrentDomain.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndCollect);
        }

    }
}
