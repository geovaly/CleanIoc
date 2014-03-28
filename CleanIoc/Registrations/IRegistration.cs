using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations
{
    interface IRegistration
    {
        void OnInitTags(ITagsInitializer initializer);

        void OnInitSingletons(ISingletonsInitializer initializer);

        void OnMakingConstants(IConstantFactory factory);

        void OnContainerWasBuilt(Container container);

        void Accept(IRegistrationVisitor visitor);
    }

    interface IRegistration<out TServie> : IRegistration
        where TServie : class
    {
        InstanceLookup<TServie> MakeInstanceLookup();

        InstanceExpression MakeInstanceExpression();
    }
}
