namespace CleanIoc.Registrations
{
    interface ISingletonsInitializer
    {
        void AddSingletonFor(object tag);

        int GetIndexOfLastAddedSingletonFor(object tag);
    }
}
