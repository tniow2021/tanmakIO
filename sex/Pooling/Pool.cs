namespace sex.Pooling
{
    public class Pool<T> : IPool<T> where T : class
    {
        T[] arr;
        bool IsMultiLayer = false;
        int top = -1;
        Func<T> constructor;
        public PoolStatistics statistics = new PoolStatistics();
        public Pool(Func<T> constructor, int n)
        {
            this.constructor = constructor;

            //T가 MultilayerPoolingObjects의 파생이면 IsMultiLayer를 true로 설정
            if (typeof(T).IsSubclassOf(typeof(MultilayerPoolingObjects)))
            {
                IsMultiLayer = true;
            }
            arr = new T[n];
            if (arr == null)
                throw new Exception("pool error 1");

            for (int i = 0; i < n; i++)
            {
                var t = constructor();
                if (t != null)
                {
                    top++;
                    arr[i] = t;
                }
                else
                    throw new Exception("pool error 2-2");
            }

            statistics.allNewCount = top + 1;
            statistics.maxCount = top + 1;
            statistics.minCount = top + 1;
        }
        public T GetBlock()
        {
            if (top + 1 > 0)
            {
                T t = arr[top];
                if (IsMultiLayer)
                {
                    ((MultilayerPoolingObjects)t).Assemble();
                }

                top--;
                if (statistics.minCount > top + 1)
                    statistics.minCount = top + 1;
                return t;
            }
            else
            {
                statistics.emptyErrorCount++;
                statistics.allNewCount++;
                return constructor();
            }
        }
        public void RepayBlock(T t)
        {
            if (t != null)
            {
                if (top >= arr.Length - 1)
                {
                    statistics.maxErrorCount++;
                    return;
                }
                top++;
                if (IsMultiLayer)
                    ((MultilayerPoolingObjects)t).Disassemble();

                arr[top] = t;
                if (statistics.maxCount < top + 1)
                    statistics.maxCount = top + 1;
            }
        }
        public PoolStatistics GetStatistics()
        {
            return statistics;
        }
    }
}
