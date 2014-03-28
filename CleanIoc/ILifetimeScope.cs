using System;

namespace CleanIoc
{
    public interface ILifetimeScope : IDisposable
    {
        TService Resolve<TService>() where TService : class;

        object Resolve(Type serviceType);

        ILifetimeScope BeginScope();

        ILifetimeScope BeginScope(object tag);

        ILifetimeScope BeginScope(object tag, object state);

        ILifetimeScope Parent { get; }

        object State { get; }

        object Tag { get; }
    }
}
