using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CleanIoc.Expressions
{
    struct InstanceExpression
    {

        private static readonly VariableExpression[] EmptyVariables = new VariableExpression[0];

        private readonly Expression _instance;
        private readonly VariableExpression[] _variables;

        public InstanceExpression(Expression instance)
            : this(instance, EmptyVariables)
        {
        }

        public Expression Instance { get { return _instance; } }

        public IReadOnlyList<VariableExpression> Variables { get { return _variables; } }

        public InstanceExpression AddVariables(params VariableExpression[] variables)
        {
            return AddVariables((IEnumerable<VariableExpression>)variables);
        }

        public InstanceExpression AddVariables(IEnumerable<VariableExpression> variables)
        {
            return new InstanceExpression(_instance, DistinctConcat(_variables, variables));
        }

        public InstanceExpression AddVariablesFrom(IEnumerable<InstanceExpression> expressions)
        {
            return AddVariables(expressions.SelectMany(s => s.Variables));
        }

        private InstanceExpression(
            Expression instance,
            VariableExpression[] variables)
        {
            _instance = instance;
            _variables = variables;
        }

        private static T[] DistinctConcat<T>(IEnumerable<T> item1, IEnumerable<T> item2)
        {
            return item1.Concat(item2).Distinct().ToArray();
        }

    }
}
