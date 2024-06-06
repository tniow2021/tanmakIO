using sex.Networking;

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

        Type multiLayerType = typeof(PoolingObjects);
        public IPool<T> SingleLayerCreatePool<T>(Func<T> Constructor, int n, int id) where T : class
        {
            //var d=Constructor as Func<PoolingObjects>;
            IPool<T> p;
            p = new BasicPool<T>(Constructor, n);
            RegisterPool(typeof(T), id, p.GetStatistics());
            return p;
        }
        //public IPool<T> MultiLayerCreatePool<T>(Func<T> Constructor, int n, int id) where T : class,PoolingObjects
        //{
        //    IPool<T> p;
        //    p = new Pool<T>(Constructor, n);
        //    RegisterPool(typeof(T), id, p.GetStatistics());
        //    return p;
        //}
        int idOffset = 0;
        public IPool<T>CreateBasicTypePool<T>(Func<T> constructor, int n)
        {
            IPool<T> p;
            p = new BasicPool<T>(constructor, n);
            RegisterPool(typeof(T), idOffset, p.GetStatistics());
            idOffset++;
            return p;
        }
        public IPool<T> CreatePool<T>(Func<T> constructor, int n) where T:PoolingObjects
        {
            IPool<T> p;
            p = new Pool<T>(constructor, n);
            RegisterPool(typeof(T), idOffset, p.GetStatistics());
            idOffset++;
            return p;
        }
        void RegisterPool(Type poolType, int id, PoolStatistics statistics)
        {
            poolDatas.Add(new PoolData(poolType, id, statistics));
        }
    }
}
