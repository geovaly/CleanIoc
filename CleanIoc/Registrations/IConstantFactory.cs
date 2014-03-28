namespace CleanIoc.Registrations
{
    interface IConstantFactory
    {
        IConstant MakeConstant<TValue>(TValue value)
            where TValue : class;
    }
}
