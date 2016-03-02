using System;
using System.Collections.Generic;
using FilterMe.ExpressionProviders;

namespace FilterMe
{
    internal class FilterTypeInfo
    {
        public FilterTypeInfo(Type targetType, Dictionary<string, IExpressionProvider> memberExpressionProviders)
        {
            MemberExpressionProviders = memberExpressionProviders;
            TargetType = targetType;
        }

        public Type TargetType { get; private set; }

        public Dictionary<string, IExpressionProvider> MemberExpressionProviders { get; private set; }
    }
}