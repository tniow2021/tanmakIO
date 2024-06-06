namespace sex.Pooling
{
    public class Pool<T> : IPool<T> where T:PoolingObjects
    {
        T[] arr;
        int top = -1;
        Func<T> constructor;
        public PoolStatistics statistics = new PoolStatistics();
        public Pool(Func<T> constructor, int n)
        {
            this.constructor = constructor;
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
                t.Assemble();
                

                top--;
                if (statistics.minCount > top + 1)
                    statistics.minCount = top + 1;
                return t;
            }
            else
            {
                statistics.emptyErrorCount++;
                statistics.allNewCount++;
                var t=constructor();
                t.Assemble();
                return t;
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

                t.Disassemble();

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
    public class qPool<T> : IPool<T> where T : PoolingObjects
    {
        T[] arr;
        UInt32 w;//미래에 쓸 위치
        UInt32 nSpace;//데이터양
        UInt32 L;
        Func<T> constructor;
        public PoolStatistics statistics = new PoolStatistics();
        public qPool(Func<T> constructor, int n)
        {
            this.constructor = constructor;
            arr = new T[n];
            if (arr == null)
                throw new Exception("pool error 1");
            if(n>Int32.MaxValue)
                throw new Exception("pool error 1-2");

            for (int i = 0; i < n; i++)
            {
                var t = constructor();
                if (t != null)
                {
                    w++;
                    arr[i] = t;
                }
                else
                    throw new Exception("pool error 2-2");
            }
            L=(UInt32)arr.Length;
            nSpace = L;
        }
        public T GetBlock()
        {
            if (L-nSpace > 0)//데이터량>0
            {
                UInt32 r = (w + nSpace) % L;
                nSpace+=1;
                return arr[r];
            }
            else
                return constructor();
                
        }
        public void RepayBlock(T t)
        {
            if (t is null)
            {
                return;
            }

            if (nSpace>0)//공간량>0
            {
                arr[w] = t;
                w = (w + 1) % L;
                nSpace-=1;
            }
        }
        public PoolStatistics GetStatistics()
        {
            return statistics;
        }
    }
}
