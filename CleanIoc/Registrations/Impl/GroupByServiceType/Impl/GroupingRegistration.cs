using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CleanIoc.Core;
using CleanIoc.Expressions;
using CleanIoc.Utility;

namespace CleanIoc.Registrations.Impl.GroupByServiceType.Impl
{
    abstract class GroupingRegistration<TService> : IRegistration<TService> where TService : class
    {
        private readonly object _syncRoot = new object();
        private readonly ITagRegistration<TService>[] _registrations;

        private bool _instanceLookupsAreLoaded;
        private IConstant _instanceLookupConst;

        protected GroupingRegistration(IEnumerable<ITagRegistration<TService>> registrations)
        {
            _registrations = registrations.ToArray();
        }

        public InstanceLookup<TService> MakeInstanceLookup()
        {
            LoadInstanceLookups();
            return GetInstance;
        }

        protected abstract TService GetInstance(LifetimeScope scope);

        protected abstract void OnLoadInstanceLookups(IReadOnlyCollection<ITagRegistration<TService>> registrations);

        private void LoadInstanceLookups()
        {
            if (_instanceLookupsAreLoaded)
                return;

            lock (_syncRoot)
            {
                if (_instanceLookupsAreLoaded)
                    return;

                _instanceLookupsAreLoaded = true;
                OnLoadInstanceLookups(_registrations);
            }
        }

        public void Accept(IRegistrationVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void OnInitTags(ITagsInitializer initializer)
        {
            _registrations.ForEach(x => x.OnInitTags(initializer));
        }

        public void OnInitSingletons(ISingletonsInitializer initializer)
        {
            _registrations.ForEach(x => x.OnInitSingletons(initializer));
        }

        public void OnContainerWasBuilt(Container container)
        {
            _registrations.ForEach(x => x.OnContainerWasBuilt(container));
        }

        public void OnMakingConstants(IConstantFactory factory)
        {
            _instanceLookupConst = factory.MakeConstant<InstanceLookup<TService>>(GetInstance);
            _registrations.ForEach(x => x.OnMakingConstants(NullConstantsFactory.Instance));
        }

        public InstanceExpression MakeInstanceExpression()
        {
            LoadInstanceLookups();

            return AllRegistrationsAreSingletons()
                ? MakeInstanceLookupExpressionForSingletons()
                : MakeInstanceLookupExpressionForTransients();
        }

        private bool AllRegistrationsAreSingletons()
        {
            return _registrations.All(x => x.IsSingleton);
        }

        private InstanceExpression MakeInstanceLookupExpressionForTransients()
        {
            VariableExpression instanceLookupVar = MakeInstanceLookupVariable();

            return new InstanceExpression(InvokeInstanceLookup(instanceLookupVar.Var))
                .AddVariables(instanceLookupVar);
        }

        private InstanceExpression MakeInstanceLookupExpressionForSingletons()
        {
            var instance = Expression.Variable(typeof(TService));
            var instanceVar = new VariableExpression(instance, AssignInstance(instance));

            return new InstanceExpression(instance)
                .AddVariables(instanceVar);
        }

        private VariableExpression MakeInstanceLookupVariable()
        {
            var instanceLookup = Expression.Variable(typeof(InstanceLookup<TService>));
            return new VariableExpression(instanceLookup, AssignInstanceLookup(instanceLookup));
        }

        private Expression AssignInstanceLookup(ParameterExpression instanceLookup)
        {
            return Expression.Assign(
                instanceLookup,
                _instanceLookupConst.ValueExpression);
        }

        private Expression AssignInstance(ParameterExpression instance)
        {
            return Expression.Assign(
                instance,
               InvokeInstanceLookup(_instanceLookupConst.ValueExpression));
        }

        private static MethodCallExpression InvokeInstanceLookup(Expression instanceLookup)
        {
            return Expression.Call(instanceLookup, "Invoke", null, Parameters.CurrentScope);
        }
    }
}
