namespace CleanIoc.Registrations.Impl.GroupByServiceType
{
    interface ITagRegistration<out TService> : IRegistration<TService> 
        where TService : class
    {
        int TagIndex { get; }

        bool IsSingleton { get; }
    }
}
