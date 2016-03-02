using System.Linq.Expressions;
using System.Reflection;

namespace FilterMe.ExpressionProviders
{
    internal abstract class ExpressionProvider : IExpressionProvider
    {
        protected ExpressionProvider(PropertyInfo property)
        {
            Property = property;
        }

        protected PropertyInfo Property { get; }

        public abstract Expression GetExpression(ParameterExpression parameter, object value);
    }
}