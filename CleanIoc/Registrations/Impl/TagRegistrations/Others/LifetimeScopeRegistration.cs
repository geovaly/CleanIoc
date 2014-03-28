using CleanIoc.Core;
using CleanIoc.Expressions;

namespace CleanIoc.Registrations.Impl.TagRegistrations.Others
{
    class LifetimeScopeRegistration : BaseTransientRegistration<ILifetimeScope>
    {
        public override InstanceExpression MakeInstanceExpression()
        {
            return new InstanceExpression(Parameters.CurrentScope);
        }

        public override InstanceLookup<ILifetimeScope> MakeInstanceLookup()
        {
            return scope => scope;
        }
    }
}
