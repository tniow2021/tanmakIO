public enum TypeCode
{
    UserTransform = 0
}
public static class Types
{    
    public static byte TypeCodeToByte(TypeCode tp)
    {
        return (byte)tp;
    }
    public static TypeCode ByteToTypeCode(byte b)
    {
        return (TypeCode)b;
    }
}
public interface INetStruct
{
    public TypeCode GetStructType();
    public byte[] Encoding();
    public void Decoding(byte[] data);
}
public struct UserTransform : INetStruct
{
    public float x, y;
    public UserTransform(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
    public TypeCode GetStructType() { return TypeCode.UserTransform; }
    public byte[] Encoding()
    {
        byte[] data = new byte[4 * 2];
        System.BitConverter.GetBytes(x).CopyTo(data, 0);
        System.BitConverter.GetBytes(y).CopyTo(data, 4);
        return data;
    }
    public void Decoding(byte[] data)
    {
        x = System.BitConverter.ToSingle(data, 0);
        y = System.BitConverter.ToSingle(data, 4);
    }
}