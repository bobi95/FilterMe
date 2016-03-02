using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FilterMe.Helpers;

namespace FilterMe.ExpressionProviders
{
    class IntExpressionProvider : ExpressionProvider
    {
        private readonly BinaryComparison action;

        public IntExpressionProvider(PropertyInfo property, BinaryComparison action) : base(property)
        {
            this.action = action;
        }

        public override Expression GetExpression(ParameterExpression parameter, object value)
        {
            return ExpressionHelpers.ComparisonExpression(action, parameter, Property.Name, value);
        }
    }
}
