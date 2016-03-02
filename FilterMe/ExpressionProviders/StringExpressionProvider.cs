using System.Linq.Expressions;
using System.Reflection;
using FilterMe.Helpers;

namespace FilterMe.ExpressionProviders
{
    class StringExpressionProvider : ExpressionProvider
    {
        private readonly StringAction action;

        public StringExpressionProvider(PropertyInfo property, StringAction action)
            : base(property)
        {
            this.action = action;
        }

        public override Expression GetExpression(ParameterExpression parameter, object value)
        {
            switch (action)
            {
                case StringAction.Starts:
                    return StringStartsWithExpression(parameter, Property.Name, value);

                case StringAction.Ends:
                    return StringEndsWithExpression(parameter, Property.Name, value);

                case StringAction.Contains:
                    return StringContainsExpression(parameter, Property.Name, value);

                default:
                    return ExpressionHelpers.EqualsExpression(parameter, Property.Name, value);
            }
        }

        private static MethodCallExpression StringContainsExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return ExpressionHelpers.MethodCallExpression(method, param, property, value);
        }

        private static MethodCallExpression StringStartsWithExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            return ExpressionHelpers.MethodCallExpression(method, param, property, value);
        }

        private static MethodCallExpression StringEndsWithExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            return ExpressionHelpers.MethodCallExpression(method, param, property, value);
        }
    }
}