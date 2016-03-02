using System;

namespace FilterMe.Attributes
{
    public abstract class FilterAttribute : Attribute
    {
        private object _defaultValue;

        public object DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value ?? string.Empty; }
        }

        public bool IsDefaultValue(object value)
        {
            return Equals(_defaultValue, value);
        }
    }
}