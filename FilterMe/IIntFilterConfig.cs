namespace FilterMe
{
    public interface IIntFilterConfig
    {
        void IntEquals();
        void IntLessThan();
        void IntGreaterThan();
        void IntLessThanOrEqual();
        void IntGreaterThanOrEqual();
    }
}