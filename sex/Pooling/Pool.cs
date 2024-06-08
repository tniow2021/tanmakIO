namespace sex.Pooling
{
    public class Pool<T> : IPool<T> where T : PoolingObjects
    {
        T[] arr;
        UInt32 count;
        public UInt32 GetCount() { return count; }
        UInt32 lastIndex;
        Func<T> constructor;
        public PoolStatistics statistics = new PoolStatistics();
        public Pool(Func<T> constructor, int n)
        {
            this.constructor = constructor;
            arr = new T[n];
            if (arr == null)
                throw new Exception("pool error 1");

            count = 0;
            for (int i = 0; i < n; i++)
            {
                var t = constructor();
                if (t != null)
                {
                    count++;
                    arr[i] = t;
                }
                else
                    throw new Exception("pool error 2-2");
            }
            lastIndex = (UInt32)arr.Length - 1;

            statistics.allNewCount =count;
            statistics.maxCount = count;
            statistics.minCount = count;
        }
        public T GetBlock()
        {
            if (count > 0)
            {
                T t = arr[count - 1];
                count--;
                t.Assemble();

                if (statistics.minCount > count)
                    statistics.minCount = count;
                return t;
            }
            else
            {
                statistics.emptyErrorCount++;
                statistics.allNewCount++;
                var t = constructor();
                return t;
            }
        }
        public void RepayBlock(T t)
        {
            if (t != null)
            {
                if (count > lastIndex)
                {
                    statistics.maxErrorCount++;
                    return;
                }
                t.Disassemble();
                arr[count] = t;
                count++;

                if (statistics.maxCount < count)
                    statistics.maxCount = count;
            }
        }
        public PoolStatistics GetStatistics()
        {
            return statistics;
        }
    }
}
    //public class Poo2l<T> : IPool<T> where T:PoolingObjects
    //{
    //    T[] arr;
    //    int top = -1;
    //    Func<T> constructor;
    //    public PoolStatistics statistics = new PoolStatistics();
    //    public Pool(Func<T> constructor, int n)
    //    {
    //        this.constructor = constructor;
    //        arr = new T[n];
    //        if (arr == null)
    //            throw new Exception("pool error 1");

    //        for (int i = 0; i < n; i++)
    //        {
    //            var t = constructor();
    //            if (t != null)
    //            {
    //                top++;
    //                arr[i] = t;
    //            }
    //            else
    //                throw new Exception("pool error 2-2");
    //        }

    //        statistics.allNewCount = top + 1;
    //        statistics.maxCount = top + 1;
    //        statistics.minCount = top + 1;
    //    }
    //    public T GetBlock()
    //    {
    //        if (top + 1 > 0)
    //        {
    //            T t = arr[top];
    //            t.Assemble();
                

    //            top--;
    //            if (statistics.minCount > top + 1)
    //                statistics.minCount = top + 1;
    //            return t;
    //        }
    //        else
    //        {
    //            statistics.emptyErrorCount++;
    //            statistics.allNewCount++;
    //            var t=constructor();
    //            t.Assemble();
    //            return t;
    //        }
    //    }
    //    public void RepayBlock(T t)
    //    {
    //        if (t != null)
    //        {
    //            if (top >= arr.Length - 1)
    //            {
    //                statistics.maxErrorCount++;
    //                return;
    //            }
    //            top++;

    //            t.Disassemble();

    //            arr[top] = t;
    //            if (statistics.maxCount < top + 1)
    //                statistics.maxCount = top + 1;
    //        }
    //    }
    //    public PoolStatistics GetStatistics()
    //    {
    //        return statistics;
    //    }
    //}