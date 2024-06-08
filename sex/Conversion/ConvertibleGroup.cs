using sex.DataStructure;
using sex.Pooling;

namespace sex.Conversion
{
    public class ConvertibleGroup
    {
        Table<IPool<IPool<Convertible>>> superPoolTable;
        IPool<IPool<Convertible>> poolOfPool;
        IPool<Convertible>[] poolArray;
        short maxNumber;
        public ConvertibleGroup(Table<IPool<IPool<Convertible>>> superPoolTable, IPool<IPool<Convertible>>poolOfPool)
        {
            this.superPoolTable = superPoolTable;
            this.poolOfPool = poolOfPool;
            poolArray = new IPool<Convertible>[superPoolTable.maxNumber+1];

            if (superPoolTable.maxNumber > short.MaxValue)
                throw new Exception();
            maxNumber =(short) superPoolTable.maxNumber;

            for (int i = 0; i < superPoolTable.maxNumber; i++)
            {
                lock (poolOfPool)
                {
                    poolArray[i] = poolOfPool.GetBlock();
                }
                var pool = superPoolTable.Get(i);
                lock (pool)
                {
                    poolArray[i] = pool.GetBlock();
                }
            }
        }
        //public void Assemble()
        //{
        //    for (int i = 0; i < superPoolTable.maxNumber; i++)
        //    {
        //        lock(poolOfPool)
        //        {
        //            poolArray[i] = poolOfPool.GetBlock();
        //        }
        //        var pool = superPoolTable.Get(i);
        //        lock(pool)
        //        {
        //            poolArray[i] = pool.GetBlock();
        //        }
        //    }
        //}
        //public void Disassemble()
        //{
        //    for (int i = 0; i < superPoolTable.maxNumber; i++)
        //    {
        //        lock (poolOfPool)
        //        { 
        //            poolOfPool.RepayBlock(poolArray[i]);
        //        }
        //        var pool = superPoolTable.Get(i);
        //        lock (pool)
        //        {
        //            pool.RepayBlock(poolArray[i]);
        //        }
        //    }
        //}

        //쓰레드안정성.....
        public Convertible GetBlock(int typeNumber)
        {
            if (typeNumber > maxNumber)
            {
                throw new Exception();
            }
            return poolArray[typeNumber].GetBlock();
        }
        public void ReturnBlock(Convertible c)
        {
            poolArray[c.GetTypeNumber()].RepayBlock(c);
        }
    }
}
