﻿namespace sex.Pooling
{
    public class PoolStatistics
    {
        public UInt32 minCount;
        public UInt32 maxCount;
        public UInt32 emptyErrorCount;
        public UInt32 allNewCount;
        public UInt32 maxErrorCount;
        public PoolStatistics()
        {
            minCount = 0; maxCount = 0; emptyErrorCount = 0; allNewCount = 0; maxErrorCount = 0;
        }
        //public PoolStatistics(int minCount, int maxCount, int emptyErrorCount, int allNewCount)
        //{
        //    this.maxCount = maxCount;
        //    this.minCount = minCount;
        //    this.emptyErrorCount = emptyErrorCount;
        //    this.allNewCount = allNewCount;
        //}
    }
}
