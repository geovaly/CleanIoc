using CleanIoc.Core;
using CleanIoc.Registrations;

namespace CleanIoc.Factory
{
    interface IInstanceLookupsBuilder : IRegistrationVisitor
    {
        IInstanceLookupFinder Build();
    }
}
