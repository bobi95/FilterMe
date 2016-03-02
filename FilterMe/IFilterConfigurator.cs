using System;
using System.Linq.Expressions;

namespace FilterMe
{
    public interface IFilterConfigurator<T>
    {
        IIntFilterConfig FilterForProperty(Expression<Func<T, int>> propertyExpression);
        IGeneralFilterConfig FilterForProperty<TVal>(Expression<Func<T, TVal>> propertyExpression)
            where TVal : struct;
        IStringFilterConfig FilterForProperty(Expression<Func<T, string>> stringPropertyExpression);
    }
}