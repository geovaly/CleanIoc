namespace CleanIoc.Registrations
{
    interface IRegistrationVisitor
    {
        void Visit<TService>(IRegistration<TService> registration)
            where TService : class;
    }
}
