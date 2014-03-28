namespace CleanIoc.Core
{
    class Container : LifetimeScope, IContainer
    {
        public static readonly object RootTag = "root";

        public Container(
            object[] constants,
            IInstanceLookupFinder instanceLookupFinder,
            ITagDetailsFinder tagDetailsFinder)
            : base(RootTag, null, null, constants, instanceLookupFinder, tagDetailsFinder)
        {
        }
    }
}
