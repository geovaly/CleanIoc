using CleanIoc.Core;

namespace CleanIoc.Factory
{
    interface ITagDetailsBuilder
    {
        TagDetails this[object tag] { get; set; }

        ITagDetailsFinder Build();
    }
}
