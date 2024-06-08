﻿namespace sex.Pooling
{
    public class BasicPool<T> :IPool<T> //where T : class
    {
        T[] arr;
        public UInt32 count;
        UInt32 lastIndex;
        Func<T> constructor;
        public PoolStatistics statistics = new PoolStatistics();
        public BasicPool(Func<T> constructor, int n)
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

            statistics.allNewCount = count;
            statistics.maxCount = count;
            statistics.minCount = count;
        }
        public T GetBlock()
        {
            if (count > 0)
            {
                T t = arr[count - 1];
                count--;

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

