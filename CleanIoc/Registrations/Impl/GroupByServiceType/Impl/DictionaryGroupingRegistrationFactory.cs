using System.Collections.Generic;

namespace CleanIoc.Registrations.Impl.GroupByServiceType.Impl
{
    class DictionaryGroupingRegistrationFactory : IGroupingRegistrationFactory
    {

        public IRegistration MakeFrom<TService>(IEnumerable<ITagRegistration<TService>> registrations)
            where TService : class
        {
            return new DictionaryGroupingRegistration<TService>(registrations);
        }
    }
}
