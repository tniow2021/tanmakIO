namespace sex.DataStructure
{
    public interface ITable<T>
    {
        public void Register(T obj, int number);
        public T Get(int number);
    }
}
