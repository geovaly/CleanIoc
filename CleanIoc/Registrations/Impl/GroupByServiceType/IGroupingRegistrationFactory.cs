using System.Collections.Generic;

namespace CleanIoc.Registrations.Impl.GroupByServiceType
{
    interface IGroupingRegistrationFactory
    {
        IRegistration MakeFrom<TService>(IEnumerable<ITagRegistration<TService>> registrations)
            where TService : class;
    }
}
