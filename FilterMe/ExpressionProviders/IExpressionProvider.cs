using System.Linq.Expressions;

namespace FilterMe.ExpressionProviders
{
    internal interface IExpressionProvider
    {
        Expression GetExpression(ParameterExpression parameter, object value);
    }
}