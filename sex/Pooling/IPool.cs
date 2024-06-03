namespace sex.Pooling
{
    public interface IPool<T> 
    {
        public T GetBlock();
        public void RepayBlock(T t);
        public PoolStatistics GetStatistics();
    }
}
