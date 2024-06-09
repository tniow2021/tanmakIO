using sex.DataStructure;
using sex.Pooling;

namespace sex.Conversion
{
    public class ConvertibleGroup
    {
        Table<IPool<IPool<INetConvertible>>> superPoolTable;
        IPool<INetConvertible>[] poolArray;
        public short maxNumber { private set; get; }
        public ConvertibleGroup(Table<IPool<IPool<INetConvertible>>> superPoolTable)
        {
            this.superPoolTable = superPoolTable;
            poolArray = new IPool<INetConvertible>[superPoolTable.maxNumber+1];

            if (superPoolTable.maxNumber > short.MaxValue)
                throw new Exception();
            maxNumber =(short) superPoolTable.maxNumber;

            for (int i = 0; i < superPoolTable.maxNumber+1; i++)
            {
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
        public INetConvertible GetBlock(int typeNumber)
        {
            if (typeNumber > maxNumber)
            {
                throw new Exception();
            }
            Console.WriteLine("dkwnwhgek" + typeNumber) ;
            return poolArray[typeNumber].GetBlock();
        }
        public void ReturnBlock(INetConvertible c)
        {
            poolArray[c.GetTypeNumber()].RepayBlock(c);
        }
    }
}
