namespace FilterMe
{
    public interface IStringFilterConfig
    {
        void StringContains();
        void StringStartsWith();
        void StringEndsWith();
        void StringEquals();
    }
}