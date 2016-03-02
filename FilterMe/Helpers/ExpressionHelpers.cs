using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FilterMe.Helpers
{
    internal static class ExpressionHelpers
    {
        public static BinaryExpression ComparisonExpression(BinaryComparison comparison, ParameterExpression param, string property, object value)
        {
            var prop = Expression.Property(param, property);
            var val = Expression.Constant(value);

            switch (comparison)
            {
                case BinaryComparison.Equals:
                    return Expression.Equal(prop, val);
                case BinaryComparison.LessThan:
                    return Expression.LessThan(prop, val);
                case BinaryComparison.GreaterThan:
                    return Expression.GreaterThan(prop, val);
                case BinaryComparison.LessThanOrEqual:
                    return Expression.LessThanOrEqual(prop, val);
                case BinaryComparison.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(prop, val);
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null);
            }
        }

        public static MethodCallExpression MethodCallExpression(MethodInfo method, ParameterExpression param, string property, object value)
        {
            MemberExpression propertyExpression = Expression.Property(param, property);
            ConstantExpression constant = Expression.Constant(value);

            return Expression.Call(propertyExpression, method, constant);
        }

        public static BinaryExpression EqualsExpression(ParameterExpression param, string property, object value)
        {
            return ComparisonExpression(BinaryComparison.Equals, param, property, value);
        }
    }
}