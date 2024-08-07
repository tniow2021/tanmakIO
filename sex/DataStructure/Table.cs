﻿namespace sex.DataStructure
{
    public class Table<T>: ITable<T>
    {
        T[] array;
        public UInt32 maxNumber { get; }
        public Table(UInt32 highestNumber)
        {
            array = new T[highestNumber + 1];
            maxNumber = highestNumber;
        }
        public void Register(T obj, int numbering)
        {
            if (numbering > maxNumber)
                throw new Exception();
            if (array[numbering] is not null)
                throw new Exception();

            array[numbering] = obj;
        }
        public IsSuccess Get(int number,out T? value)
        {
            var t = array[number];
            if (t is null)
            {
                value=default(T);
                return IsSuccess.failure;
            }
            value = t;
            return IsSuccess.Success;
        }
    }
}
