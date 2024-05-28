namespace sex.Pooling
{
    public interface IPool<T> where T : class
    {
        public T GetBlock();
        public void RepayBlock(T t);
        public PoolStatistics GetStatistics();
    }
}
