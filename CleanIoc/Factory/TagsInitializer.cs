using CleanIoc.Core;
using CleanIoc.Registrations;

namespace CleanIoc.Factory
{
    class TagsInitializer : ITagsInitializer
    {
        private readonly ITagDetailsBuilder _builder;
        private int _nextTagId;

        public TagsInitializer(ITagDetailsBuilder builder)
        {
            _builder = builder;
        }

        public void AddTagIfNotExists(object tag)
        {
            if (_builder[tag] == TagDetails.Unknown)
            {
                _builder[tag] = new TagDetails(_nextTagId++);
            }
        }

        public int GetIndexOfAddedTag(object tag)
        {
            return _builder[tag].TagIndex;
        }
    }
}