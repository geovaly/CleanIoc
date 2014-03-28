using CleanIoc.Registrations;

namespace CleanIoc.Factory
{
    interface IConstantsBuilder : IConstantFactory
    {
        object[] Build();
    }
}
