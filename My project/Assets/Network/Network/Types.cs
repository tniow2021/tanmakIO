using System;
public enum TypeCode
{
    None,
    UserTransform,
    AliveCherk
}
public static class Types
{
    static int numberOfTypeCode = System.Enum.GetValues(typeof(TypeCode)).Length;
    public static byte TypeCodeToByte(TypeCode tp)
    {
        return (byte)tp;
    }
    public static TypeCode ByteToTypeCode(byte b)
    {
        if (b >= numberOfTypeCode) return TypeCode.None;
        return (TypeCode)b;
    }
}
public interface INetStruct
{
    public TypeCode GetTypeCode();
    public byte[] Encoding();
    public void Decoding(byte[] data);
}
public struct UserTransform : INetStruct
{
    //유저 식별자
    public float x, y;
    public UserTransform(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
    public TypeCode GetTypeCode() { return TypeCode.UserTransform; }
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
public struct AliveCherk : INetStruct
{
    public TypeCode GetTypeCode() { return TypeCode.AliveCherk; }
    int ID;
    public AliveCherk(int _ID)
    {
        ID = _ID;
    }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes((Int32)ID);
    }
    public void Decoding(byte[] data)
    {
        ID = System.BitConverter.ToInt32(data, 0);
    }
}