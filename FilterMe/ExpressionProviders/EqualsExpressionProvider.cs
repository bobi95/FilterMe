using System.Linq.Expressions;
using System.Reflection;
using FilterMe.Helpers;

namespace FilterMe.ExpressionProviders
{
    internal class EqualsExpressionProvider : ExpressionProvider
    {
        public EqualsExpressionProvider(PropertyInfo property)
            : base(property)
        {
        }

        public override Expression GetExpression(ParameterExpression parameter, object value)
        {
            return ExpressionHelpers.EqualsExpression(parameter, Property.Name, value);
        }
    }
}