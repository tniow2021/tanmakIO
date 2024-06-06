using sex.Pooling;

namespace sex.Conversion
{
    public class ConvertiblePoolTable
    {
        int highestTypeNumber;
        int defaultPoolingSize;
        Func<Convertible>[] constructorArray;
        IPool<Convertible> []poolArray;
        PoolEngine poolEngine;

        public ConvertiblePoolTable(PoolEngine poolEngine,int highestTypeNumber=100,int defaultPoolingSize=100)
        {
            this.highestTypeNumber = highestTypeNumber;
            this.poolEngine = poolEngine;
            this.defaultPoolingSize = defaultPoolingSize;

            constructorArray = new Func<Convertible>[highestTypeNumber];
            poolArray=new IPool<Convertible>[highestTypeNumber];

            for (int i=0;i< highestTypeNumber; i++)
            {
                constructorArray[i] = null;
            }
        }
        public void RegisterConvertibleObject(Func<Convertible>constructor)
        {
            Convertible c=constructor();
            short typeNumber=c.GetTypeNumber();

            if (typeNumber < 0 || typeNumber > highestTypeNumber)
                throw new Exception();

            //이미 등록되어있는 경우 예외처리.
            if (constructorArray[typeNumber] is not null &&
                poolArray[typeNumber] is not null) 
            {
                throw new Exception();
            }

            constructorArray[typeNumber]=constructor;
            poolArray[typeNumber] = poolEngine.CreateBasicTypePool(constructor, defaultPoolingSize);

        }
        //쓰레드안정성.....
        public IsSuccess GetBlock(int typeNumber, out Convertible c)
        {
            if (typeNumber > highestTypeNumber)
            {
                c = null;
                return IsSuccess.failure;
            }
            else if(poolArray[typeNumber] is null)
            {
                c = null;
                return IsSuccess.failure;
            }
            c=poolArray[typeNumber].GetBlock();
            return IsSuccess.Success;
        }
        public void ReturnBlock(Convertible c)
        {
            poolArray[c.GetTypeNumber()].RepayBlock(c);
        }
    }
}
