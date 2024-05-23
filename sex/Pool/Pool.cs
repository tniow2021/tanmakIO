namespace sex.Pool
{    
    public struct PoolStatistics
    {
        public int minCount;
        public int maxCount;
        public int emptyErrorCount;
        public PoolStatistics()
        {
            minCount = 0; maxCount=0; emptyErrorCount = 0;
        }
        public PoolStatistics(int minCount, int maxCount, int emptyErrorCount)
        {
            this.maxCount = maxCount;
            this.minCount = minCount;
            this.emptyErrorCount = emptyErrorCount;
        }
    }
    public abstract class IPool <T> where T : class, new()
    {
        protected PoolStatistics statistics=new PoolStatistics();
        public abstract T GetBlock();
        public abstract void RepayBlock(T t);
        public PoolStatistics GetStatistics()
        {
            return statistics;
        }
    }

    public class Pool<T> :IPool<T> where T : class, new()
    {
        Stack<T> stack = new Stack<T>();
        public Pool(int n=100)
        {
            for(int i=0; i< n;i++)
            {
                stack.Push(new T());
            }
        }
        public override T GetBlock()
        {
            if(stack.Count > 0)
            {
                return stack.Pop();
            }
            else
            {
                statistics.emptyErrorCount++;
                return new T();
            }
        }
        public override void RepayBlock(T t)
        {
            stack.Push(t);
        }
    }
    public class PoolMoniter
    {
        struct PoolData
        {
            public int id;
            public Type type;
            public Func<PoolStatistics> GetStatistics;
            public PoolData(Type type,int id, Func<PoolStatistics> GetStatistics)
            {
                this.type = type; this.id = id; this.GetStatistics = GetStatistics;
            }
        }
        List<PoolData>poolDatas = new List<PoolData>();
        public void RegisterPool(Type poolType,int id,Func<PoolStatistics> GetStatistics)
        {
            poolDatas.Add(new PoolData(poolType, id, GetStatistics));
        }
        public void PrintPoolMoniter()
        {
            for(int i=0;i<poolDatas.Count;i++)
            {
                PoolStatistics ps = poolDatas[i].GetStatistics();
                Console.WriteLine($"id:{poolDatas[i].id} | type:{poolDatas[i].type}|minCount:{ps.minCount} |maxCount:{ps.maxCount}");
            }
            Console.WriteLine("---------------------------------------------------------------------------");
        }
    }
}
