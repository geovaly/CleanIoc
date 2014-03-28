using System.Collections.Generic;
using CleanIoc.Core;

namespace CleanIoc.Registrations
{
    class CompositeRegistration : List<IRegistration>, IRegistration
    {
        public CompositeRegistration()
        {
        }

        public CompositeRegistration(IEnumerable<IRegistration> registrations)
            : base(registrations)
        {
        }

        public void OnInitTags(ITagsInitializer initializer)
        {
            ForEach(reg => reg.OnInitTags(initializer));
        }

        public void OnInitSingletons(ISingletonsInitializer initializer)
        {
            ForEach(reg => reg.OnInitSingletons(initializer));
        }

        public void OnMakingConstants(IConstantFactory factory)
        {
            ForEach(reg => reg.OnMakingConstants(factory));
        }

        public void OnContainerWasBuilt(Container container)
        {
            ForEach(reg => reg.OnContainerWasBuilt(container));
        }

        public void Accept(IRegistrationVisitor visitor)
        {
            ForEach(reg => reg.Accept(visitor));
        }
    }
}
