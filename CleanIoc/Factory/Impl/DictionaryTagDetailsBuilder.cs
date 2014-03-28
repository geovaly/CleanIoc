using System.Collections.Generic;
using CleanIoc.Core;
using CleanIoc.Utility;

namespace CleanIoc.Factory.Impl
{
    class DictionaryTagDetailsBuilder : BaseBuilder<ITagDetailsFinder>, ITagDetailsBuilder
    {
        private readonly TagDetailsFinder _finder = new TagDetailsFinder();

        public TagDetails this[object tag]
        {
            get
            {
                EnsureWasNotBuilt();
                return _finder.Find(tag);
            }
            set
            {
                EnsureWasNotBuilt();
                _finder[tag] = value;
            }
        }

        protected override ITagDetailsFinder OnBuild()
        {
            return _finder;
        }

        private class TagDetailsFinder : Dictionary<object, TagDetails>, ITagDetailsFinder
        {
            public TagDetails Find(object tag)
            {
                TagDetails result;

                return TryGetValue(tag, out result)
                    ? result
                    : TagDetails.Unknown;
            }
        }
    }
}
