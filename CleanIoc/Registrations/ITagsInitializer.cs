namespace CleanIoc.Registrations
{
    interface ITagsInitializer
    {
        void AddTagIfNotExists(object tag);

        int GetIndexOfAddedTag(object tag);
    }
}
