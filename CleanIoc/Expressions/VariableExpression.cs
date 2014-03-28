using System;
using System.Linq.Expressions;

namespace CleanIoc.Expressions
{
    struct VariableExpression : IEquatable<VariableExpression>
    {
        public readonly ParameterExpression Var;
        public readonly Expression InitVar;

        public VariableExpression(ParameterExpression var, Expression initVar)
        {
            Var = var;
            InitVar = initVar;
        }

        public override int GetHashCode()
        {
            return (Var.Type.Name + Var.Name).GetHashCode();
        }

        public bool Equals(VariableExpression other)
        {
            return Var.Type == other.Var.Type && Var.Name == other.Var.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is VariableExpression && Equals((VariableExpression)obj);
        }

        public static bool operator ==(VariableExpression lhs, VariableExpression rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(VariableExpression lhs, VariableExpression rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
