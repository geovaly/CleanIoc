namespace CleanIoc.Core
{
    delegate TService InstanceLookup<out TService>(LifetimeScope scope) 
        where TService : class;
}
