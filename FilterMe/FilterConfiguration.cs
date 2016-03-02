using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FilterMe.ExpressionProviders;

namespace FilterMe
{
    internal class FilterConfiguration<T> : IFilterConfigurator<T>, IStringFilterConfig, IGeneralFilterConfig, IIntFilterConfig
    {
        private readonly FilterTypeInfo _filterTypeInfo;

        private PropertyInfo _currentPropertyInfo;

        public FilterConfiguration(FilterTypeInfo filterTypeInfo)
        {
            _filterTypeInfo = filterTypeInfo;
        }

        public IGeneralFilterConfig FilterForProperty<TVal>(Expression<Func<T, TVal>> propertyExpression)
            where TVal : struct 
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("Property expression needed");
            }

            GetPropertyFromExpression(propertyExpression.Body);

            return this;
        }

        public IStringFilterConfig FilterForProperty(Expression<Func<T, string>> stringPropertyExpression)
        {
            if (stringPropertyExpression == null)
            {
                throw new ArgumentNullException("Property expression needed");
            }

            GetPropertyFromExpression(stringPropertyExpression.Body);

            return this;
        }

        public IIntFilterConfig FilterForProperty(Expression<Func<T, int>> intPropertyExpression)
        {
            if (intPropertyExpression == null)
            {
                throw new ArgumentNullException("Property expression needed");
            }

            GetPropertyFromExpression(intPropertyExpression.Body);

            return this;
        }

        private void GetPropertyFromExpression(Expression expressionBody)
        {
            var body = expressionBody as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Not a parameter expression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Not a parameter expression");
            }

            _currentPropertyInfo = property;
        }

        public void MatchEquals()
        {
            var expressionProvider = new EqualsExpressionProvider(_currentPropertyInfo);

            _filterTypeInfo.MemberExpressionProviders[_currentPropertyInfo.Name] = expressionProvider;
        }

        public void StringEquals()
        {
            AddStringExpressionProvider(StringAction.Equals);
        }

        public void StringContains()
        {
            AddStringExpressionProvider(StringAction.Contains);
        }

        public void StringStartsWith()
        {
            AddStringExpressionProvider(StringAction.Starts);
        }

        public void StringEndsWith()
        {
            AddStringExpressionProvider(StringAction.Ends);
        }

        private void AddStringExpressionProvider(StringAction action)
        {
            var expressionProvider = new StringExpressionProvider(_currentPropertyInfo, action);

            _filterTypeInfo.MemberExpressionProviders[_currentPropertyInfo.Name] = expressionProvider;
        }

        public void IntEquals()
        {
            AddIntExpressionProvider(BinaryComparison.Equals);
        }

        public void IntLessThan()
        {
            AddIntExpressionProvider(BinaryComparison.LessThan);
        }

        public void IntGreaterThan()
        {
            AddIntExpressionProvider(BinaryComparison.GreaterThan);
        }

        public void IntLessThanOrEqual()
        {
            AddIntExpressionProvider(BinaryComparison.LessThanOrEqual);
        }

        public void IntGreaterThanOrEqual()
        {
            AddIntExpressionProvider(BinaryComparison.GreaterThanOrEqual);
        }

        private void AddIntExpressionProvider(BinaryComparison action)
        {
            var expressionProvider = new IntExpressionProvider(_currentPropertyInfo, action);

            _filterTypeInfo.MemberExpressionProviders[_currentPropertyInfo.Name] = expressionProvider;
        }
    }
}
