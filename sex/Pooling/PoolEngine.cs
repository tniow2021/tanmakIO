namespace sex.Pooling
{
    public class PoolEngine
    {
        //나중에 일정시간마다 statistics를 저장하게 만들기
        struct PoolData
        {
            public int id;
            public Type type;
            public PoolStatistics statistics;
            public PoolData(Type type, int id, PoolStatistics statistics)
            {
                this.type = type; this.id = id; this.statistics = statistics;
            }
        }
        List<PoolData> poolDatas = new List<PoolData>();

        public void PrintPoolStatistics()
        {
            Console.WriteLine($"PrintPoolState. pool count: {poolDatas.Count}");
            for (int i = 0; i < poolDatas.Count; i++)
            {
                PoolStatistics ps = poolDatas[i].statistics;
                Console.WriteLine($"id:{poolDatas[i].id} |" +
                    $"type:{poolDatas[i].type} |" +
                    $"minCount:{ps.minCount} |" +
                    $"maxCount:{ps.maxCount} |" +
                    $"emptyErrorCount:{ps.emptyErrorCount} |" +
                    $"allNewCount:{ps.allNewCount}|");
            }
            Console.WriteLine("---------------------------------------------------------------------------");
        }

        Type multiLayerType = typeof(MultilayerPoolingObjects);
        public IPool<T> CreatePool<T>(Func<T> Constructor, int n, int id) where T : class
        {
            IPool<T> p;
            if (typeof(T).IsSubclassOf(multiLayerType))
            {
                p = new Pool<T>(Constructor,n);
            }
            else
                p = new BasicPool<T>(Constructor, n);

            RegisterPool(typeof(T), id, p.GetStatistics());
            return p;
        }
        void RegisterPool(Type poolType, int id, PoolStatistics statistics)
        {
            poolDatas.Add(new PoolData(poolType, id, statistics));
        }
    }
}
