using System;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Delegate.Util
{
    static class TypeExtensions
    {
        public static bool CanBeDisposable(this Type serviceType)
        {
            return !serviceType.IsSealed || typeof(IDisposable).IsAssignableFrom(serviceType);
        }
    }
}
