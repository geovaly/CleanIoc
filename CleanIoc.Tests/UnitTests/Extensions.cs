using System.Collections.Generic;
using CleanIoc.Registrations;

namespace CleanIoc.Tests.UnitTests
{
    static class Extensions
    {
        public static List<IRegistration> LeafRegistrations(this IRegistration registration)
        {
            var visitor = new LeafRegistrationsVisitor();
            registration.Accept(visitor);
            return visitor;
        }

        private class LeafRegistrationsVisitor : List<IRegistration>, IRegistrationVisitor
        {
            public void Visit<TService>(IRegistration<TService> registration) where TService : class
            {
                Add(registration);
            }
        }
    }
}
