using System;
public enum TypeCode
{
    None,
    DummyData,
    TimeOutCherk,
    UserTransform,
    AliveCherk,
    AccessRequest,
    AccessRequestAnswer,
    ExitUserSignal
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
    public int ID;
    public float x, y;
    public UserTransform(float _x, float _y, int _ID)
    {
        ID = _ID;
        x = _x;
        y = _y;
    }
    public TypeCode GetTypeCode() { return TypeCode.UserTransform; }
    public byte[] Encoding()
    {
        byte[] data = new byte[4 * 3];
        System.BitConverter.GetBytes(x).CopyTo(data, 0);
        System.BitConverter.GetBytes(y).CopyTo(data, 4);
        System.BitConverter.GetBytes(ID).CopyTo(data, 8);
        return data;
    }
    public void Decoding(byte[] data)
    {
        x = System.BitConverter.ToSingle(data, 0);
        y = System.BitConverter.ToSingle(data, 4);
        ID = System.BitConverter.ToInt32(data, 8);
    }
}
public struct AccessRequest : INetStruct
{
    public int ID;
    public AccessRequest(int _ID)
    {
        ID = _ID;
    }
    public TypeCode GetTypeCode() { return TypeCode.AccessRequest; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(ID);
    }
    public void Decoding(byte[] data)
    {
        ID = System.BitConverter.ToInt32(data, 0);
    }
}
public struct AccessRequestAnswer : INetStruct
{
    public int YourID;
    public AccessRequestAnswer(int _YourID)
    {
        YourID = _YourID;
    }
    public TypeCode GetTypeCode() { return TypeCode.AccessRequestAnswer; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(YourID);
    }
    public void Decoding(byte[] data)
    {
        YourID = System.BitConverter.ToInt32(data, 0);
    }

}
public struct ExitUserSignal : INetStruct
{
    public int exitUserID;
    public ExitUserSignal(int _exitUserID)
    {
        exitUserID = _exitUserID;
    }
    public TypeCode GetTypeCode() { return TypeCode.ExitUserSignal; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(exitUserID);
    }
    public void Decoding(byte[] data)
    {
        exitUserID = System.BitConverter.ToInt32(data, 0);
    }
}
public struct TimeOutCherk : INetStruct
{
    public TypeCode GetTypeCode() { return TypeCode.TimeOutCherk; }
    public byte[] Encoding()
    {
        return new byte[0];
    }
    public void Decoding(byte[] data) { }
}
public struct DummyData : INetStruct
{
    public TypeCode GetTypeCode() { return TypeCode.DummyData; }
    public byte[] Encoding()
    {
        return new byte[0];
    }
    public void Decoding(byte[] data) { }
}