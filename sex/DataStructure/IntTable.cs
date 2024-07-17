namespace sex.DataStructure
{
    public class IntTable : ITable<int>
    {
        int[] array;
        public UInt32 maxNumber { get; }
        public IntTable(UInt32 highestNumber)
        {
            array = new int[highestNumber + 1];
            maxNumber = highestNumber;
        }
        public void Register(int obj, int numbering)
        {
            if (numbering > maxNumber)
                throw new Exception();
            if (array[numbering] != 0)
                throw new Exception();

            array[numbering] = obj;
        }
        public IsSuccess Get(int number,out int value)
        {
            int t = array[number];
            if (t == 0)
            {
                value = 0;
                return IsSuccess.failure;
            }
            value = array[t];
            return IsSuccess.Success;
        }
    }
}
