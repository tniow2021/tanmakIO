namespace sex.Pooling
{
    public class PoolStatistics
    {
        public int minCount;
        public int maxCount;
        public int emptyErrorCount;
        public int allNewCount;
        public int maxErrorCount;
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
