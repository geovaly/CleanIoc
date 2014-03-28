using CleanIoc.Builder;

namespace CleanIoc.Tests.AcceptanceTests
{
    abstract class BaseTests
    {
        protected virtual ContainerBuilder AContainer()
        {
            return new ContainerBuilder();
        }
    }
}
