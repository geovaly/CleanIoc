using System;
using System.Reflection;

namespace CleanIoc.Policy
{
    interface IConstructorSelectorPolicy
    {
        ConstructorInfo SelectConstructor(Type type);
    }
}
