using CleanIoc.Registrations;

namespace CleanIoc.Factory
{
    class SingletonsInitializer : ISingletonsInitializer
    {
        private readonly ITagDetailsBuilder _builder;

        public SingletonsInitializer(ITagDetailsBuilder builder)
        {
            _builder = builder;
        }

        public void AddSingletonFor(object tag)
        {
            _builder[tag] = _builder[tag].AddSingleton();
        }

        public int GetIndexOfLastAddedSingletonFor(object tag)
        {
            return _builder[tag].LastSingletonIndex();
        }
    }
}