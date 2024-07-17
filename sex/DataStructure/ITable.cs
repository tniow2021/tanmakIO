namespace sex.DataStructure
{
    public interface ITable<T>
    {
        public void Register(T obj, int number);
        public IsSuccess Get(int number,out T value);
    }
}
