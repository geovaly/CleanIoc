using System;
using System.Reflection;

namespace CleanIoc.Policy.Impl
{
    class SingleConstructorSelectorPolicy : IConstructorSelectorPolicy
    {
        public ConstructorInfo SelectConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            if (constructors.Length == 0)
                throw new BadConfigurationException(
                    string.Format(ExceptionMessages.TypeWithNoPublicConstructor, type));

            if (constructors.Length > 1)
                throw new BadConfigurationException(
                    string.Format(ExceptionMessages.TypeWithManyPublicConstructors, type));

            return constructors[0];
        }
    }
}
