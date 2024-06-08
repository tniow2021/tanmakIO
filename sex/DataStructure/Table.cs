namespace sex.DataStructure
{
    public class Table<T>: ITable<T>
    {
        T[] array;
        public UInt32 maxNumber { get; }
        public Table(UInt32 highestNumber = 100)
        {
            array = new T[highestNumber + 1];
            maxNumber = highestNumber;
        }
        public void Register(T obj, int number)
        {
            if (number > maxNumber)
                throw new Exception();
            if (array[number] is not null)
                throw new Exception();

            array[number] = obj;
        }
        public T Get(int number)
        {
            var t = array[number];
            if (t is null)
                throw new Exception();
            return t;
        }
    }
}
