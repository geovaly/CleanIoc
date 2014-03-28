namespace CleanIoc.Registrations.Impl.TagRegistrations
{
    abstract class BaseTransientRegistration<TService> : InitTagsRegistration<TService> where TService : class
    {
        protected BaseTransientRegistration()
        {
        }

        protected BaseTransientRegistration(object tag)
            : base(tag)
        {
        }

        public override bool IsSingleton
        {
            get { return false; }
        }
    }
}
