using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FilterMe.Attributes;

namespace FilterMe
{
    public class FilterFactory
    {
        public static Expression<Func<T, bool>> GetFilter<T>(IDictionary<string, object> filterItems, string paramName = "param")
        {
            int count = filterItems.Count;
            Type elemenType = typeof(T);

            if (count == 0)
            {
                return null;
            }

            var param = Expression.Parameter(typeof(T), string.IsNullOrWhiteSpace(paramName) ? "param" : paramName);
            Expression body = null;

            if (count == 1)
            {
                var elem = filterItems.First();
                body = GetExpressionForProperty(elemenType, param, elem.Key, elem.Value);
            }
            else
            {
                var items = filterItems.ToArray();

                Stack<Expression> expressions = new Stack<Expression>();

                for (int i = 0; i < count; i++)
                {
                    var elem = items[i];

                    var expr = GetExpressionForProperty(elemenType, param, elem.Key, elem.Value);
                    if (expr != null)
                    {
                        expressions.Push(expr);
                    }
                }

                count = expressions.Count;
                for(var i = 0; i < count; i++)
                {
                    var e = expressions.Pop();

                    body = body == null ? e : Expression.And(e, body);
                }
            }

            if (body == null) return null;

            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private static Expression GetExpressionForProperty(Type pagedViewType, ParameterExpression param, string property,
            object value)
        {
            var propertyInfo = pagedViewType.GetProperty(property);

            if (propertyInfo == null)
            {
                return null;
            }

            var attrs =
                propertyInfo
                    .GetCustomAttributes()
                    .FirstOrDefault(a => a.GetType().IsSubclassOf(typeof(FilterAttribute)));

            if (attrs == null)
            {
                return EqualsExpression(param, property, value);
            }

            if (attrs is StringFilterAttribute)
            {
                var strAttr = attrs as StringFilterAttribute;

                switch (strAttr.Action)
                {
                    case StringAction.Starts:
                        return StringStartsWithExpression(param, property, value);

                    case StringAction.Ends:
                        return StringEndsWithExpression(param, property, value);

                    case StringAction.Contains:
                        return StringContainsExpression(param, property, value);

                    default:
                        return EqualsExpression(param, property, value);
                }
            }

            return null;
        }

        private static MethodCallExpression MethodCallExpression(MethodInfo method, ParameterExpression param,
            string property, object value)
        {
            MemberExpression propertyExpression = Expression.Property(param, property);
            ConstantExpression constant = Expression.Constant(value);

            return Expression.Call(propertyExpression, method, constant);
        }

        private static MethodCallExpression StringContainsExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return MethodCallExpression(method, param, property, value);
        }

        private static MethodCallExpression StringStartsWithExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            return MethodCallExpression(method, param, property, value);
        }

        private static MethodCallExpression StringEndsWithExpression(ParameterExpression param, string property, object value)
        {
            MethodInfo method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            return MethodCallExpression(method, param, property, value);
        }

        private static BinaryExpression EqualsExpression(ParameterExpression param, string property, object value)
        {
            return Expression.Equal(Expression.Property(param, property), Expression.Constant(value));
        }
    }
}