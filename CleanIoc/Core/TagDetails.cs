namespace CleanIoc.Core
{
    struct TagDetails
    {
        public static readonly TagDetails Unknown = new TagDetails(-1);

        public readonly int TagIndex;
        public readonly int SingletonsCount;

        public TagDetails(int tagIndex)
            : this(tagIndex, 0)
        {
        }

        public TagDetails(int tagIndex, int singletonsCount)
        {
            TagIndex = tagIndex;
            SingletonsCount = singletonsCount;
        }

        public TagDetails AddSingleton()
        {
            return new TagDetails(TagIndex, SingletonsCount + 1);
        }

        public int LastSingletonIndex()
        {
            return SingletonsCount - 1;
        }

        public override int GetHashCode()
        {
            return TagIndex;
        }

        public bool Equals(TagDetails other)
        {
            return TagIndex == other.TagIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is TagDetails && Equals((TagDetails)obj);
        }

        public static bool operator ==(TagDetails lhs, TagDetails rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TagDetails lhs, TagDetails rhs)
        {
            return !lhs.Equals(rhs);
        }

    }
}
