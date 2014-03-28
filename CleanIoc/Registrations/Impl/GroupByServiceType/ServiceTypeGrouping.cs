using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanIoc.Registrations.Impl.GroupByServiceType
{
    class ServiceTypeGrouping
    {
        private readonly IGroupingRegistrationFactory _factory;

        public ServiceTypeGrouping(IGroupingRegistrationFactory factory)
        {
            _factory = factory;
        }

        public IRegistration Group(IRegistration registration)
        {
            var visitor = new GroupByGroupingVisitor();
            registration.Accept(visitor);
            return visitor.Build(_factory);
        }

        interface IGroupingRegistrationBuilder
        {
            IRegistration Build(IGroupingRegistrationFactory factory);
        }

        class GroupByGroupingVisitor : Dictionary<Type, IGroupingRegistrationBuilder>,
            IGroupingRegistrationBuilder, IRegistrationVisitor
        {
            public void Visit<TService>(IRegistration<TService> registration)
                where TService : class
            {
                AddNewBuilderIfNotExists<TService>();
                GetBuilder<TService>().Add((ITagRegistration<TService>)registration);
            }

            private void AddNewBuilderIfNotExists<TService>() where TService : class
            {
                if (ContainsKey(typeof(TService)))
                    return;

                Add(typeof(TService), new GroupingRegistrationBuilder<TService>());
            }

            private GroupingRegistrationBuilder<TService> GetBuilder<TService>()
                where TService : class
            {
                return (GroupingRegistrationBuilder<TService>)this[typeof(TService)];
            }

            public IRegistration Build(IGroupingRegistrationFactory factory)
            {
                return new CompositeRegistration(Values.Select(x => x.Build(factory)));
            }
        }

        private class GroupingRegistrationBuilder<TService>
            : List<ITagRegistration<TService>>, IGroupingRegistrationBuilder
            where TService : class
        {
            public IRegistration Build(IGroupingRegistrationFactory factory)
            {
                return Count > 1
                    ? factory.MakeFrom(this)
                    : this[0];
            }
        }


    }
}
