using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FilterMe.ExpressionProviders;

namespace FilterMe
{
    public static class FilterContext
    {
        private static Dictionary<Type, FilterTypeInfo> filterTypeInfos = new Dictionary<Type, FilterTypeInfo>();

        public static void MapType<T>(Action<IFilterConfigurator<T>> configuration)
        {
            var config = new FilterConfiguration<T>(GetTargetTypeInfo(typeof(T)));

            configuration(config);
        }

        public static Expression<Func<T, bool>> GetFilter<T>(IDictionary<string, object> filterItems, string paramName = "x")
        {
            int count = filterItems.Count;
            Type elemenType = typeof(T);
            var exprProviders = GetTargetTypeInfo(elemenType).MemberExpressionProviders;

            if (count == 0)
            {
                return null;
            }

            var param = Expression.Parameter(typeof(T), string.IsNullOrWhiteSpace(paramName) ? "x" : paramName);
            Expression body = null;

            if (count == 1)
            {
                var elem = filterItems.First();
                body = exprProviders.ContainsKey(elem.Key)
                    ? exprProviders[elem.Key].GetExpression(param, elem.Value)
                    : null;
            }
            else
            {
                var items = filterItems.ToArray();

                Stack<Expression> expressions = new Stack<Expression>();

                for (int i = 0; i < count; i++)
                {
                    var elem = items[i];

                    var expr = exprProviders.ContainsKey(elem.Key)
                    ? exprProviders[elem.Key].GetExpression(param, elem.Value)
                    : null;

                    if (expr != null)
                    {
                        expressions.Push(expr);
                    }
                }

                count = expressions.Count;
                for (var i = 0; i < count; i++)
                {
                    var e = expressions.Pop();

                    body = body == null ? e : Expression.And(e, body);
                }
            }

            if (body == null) return null;

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private static FilterTypeInfo GetTargetTypeInfo(Type targetType)
        {
            if (filterTypeInfos.ContainsKey(targetType))
            {
                return filterTypeInfos[targetType];
            }

            var typeInfo = new FilterTypeInfo(targetType, new Dictionary<string, IExpressionProvider>());
            filterTypeInfos[targetType] = typeInfo;

            return typeInfo;
        }
    }
}
